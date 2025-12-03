using System.Net.Mime;
using Backend.API.Groups.Domain.Model.Aggregates;
using Backend.API.Groups.Domain.Repositories;
using Backend.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Backend.API.Groups.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Colleague endpoints")]
public class ColleaguesController(IColleagueRepository colleagueRepository) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get all colleagues for the authenticated user")]
    public async Task<IActionResult> GetAll()
    {
        var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
        var colleagues = await colleagueRepository.GetAllByUserIdAsync(userId);
        return Ok(colleagues);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a specific colleague by ID")]
    public async Task<IActionResult> GetById(int id)
    {
        var colleague = await colleagueRepository.GetByIdAsync(id);
        if (colleague == null)
            return NotFound(new { message = "Colleague not found" });
        return Ok(colleague);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new colleague")]
    public async Task<IActionResult> Create([FromBody] CreateColleagueDto request)
    {
        var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
        var colleague = new Colleague(request.Name, request.Email, request.Phone, userId, request.GroupId)
        {
            IsLeader = request.IsLeader
        };
        
        if (request.Address != null)
        {
            colleague.Address = new Backend.API.Groups.Domain.Model.ValueObjects.Address(
                request.Address.Street,
                request.Address.City,
                request.Address.Latitude,
                request.Address.Longitude
            );
        }

        var createdColleague = await colleagueRepository.AddAsync(colleague);
        return CreatedAtAction(nameof(GetById), new { id = createdColleague.Id }, createdColleague);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an existing colleague")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateColleagueDto request)
    {
        var colleague = await colleagueRepository.GetByIdAsync(id);
        if (colleague == null)
            return NotFound(new { message = "Colleague not found" });

        colleague.UpdateName(request.Name);
        colleague.UpdateEmail(request.Email);
        colleague.UpdatePhone(request.Phone);
        colleague.UpdateLeaderStatus(request.IsLeader);

        if (request.Address != null)
        {
            colleague.UpdateAddress(new Backend.API.Groups.Domain.Model.ValueObjects.Address(
                request.Address.Street,
                request.Address.City,
                request.Address.Latitude,
                request.Address.Longitude
            ));
        }

        var updated = await colleagueRepository.UpdateAsync(colleague);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a colleague")]
    public async Task<IActionResult> Delete(int id)
    {
        var colleague = await colleagueRepository.GetByIdAsync(id);
        if (colleague == null)
            return NotFound(new { message = "Colleague not found" });

        await colleagueRepository.DeleteAsync(id);
        return NoContent();
    }
}

public class AddressDto
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class CreateColleagueDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int? GroupId { get; set; }
    public bool IsLeader { get; set; }
    public AddressDto? Address { get; set; }
}

public class UpdateColleagueDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsLeader { get; set; }
    public AddressDto? Address { get; set; }
}


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
[SwaggerTag("Available Group endpoints")]
public class GroupsController(IGroupRepository groupRepository) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get all groups for the authenticated user")]
    [SwaggerResponse(StatusCodes.Status200OK, "Groups retrieved successfully")]
    public async Task<IActionResult> GetAll()
    {
        var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
        var groups = await groupRepository.GetAllByUserIdAsync(userId);
        return Ok(groups);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a specific group by ID")]
    [SwaggerResponse(StatusCodes.Status200OK, "Group retrieved successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Group not found")]
    public async Task<IActionResult> GetById(int id)
    {
        var group = await groupRepository.GetByIdAsync(id);
        if (group == null)
            return NotFound(new { message = "Group not found" });
        return Ok(group);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new group")]
    [SwaggerResponse(StatusCodes.Status201Created, "Group created successfully")]
    public async Task<IActionResult> Create([FromBody] CreateGroupRequest request)
    {
        var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
        var group = new Group(request.Name, request.Description, request.Color, userId);
        var createdGroup = await groupRepository.AddAsync(group);
        return CreatedAtAction(nameof(GetById), new { id = createdGroup.Id }, createdGroup);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an existing group")]
    [SwaggerResponse(StatusCodes.Status200OK, "Group updated successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Group not found")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateGroupRequest request)
    {
        var group = await groupRepository.GetByIdAsync(id);
        if (group == null)
            return NotFound(new { message = "Group not found" });

        group.UpdateName(request.Name);
        group.UpdateDescription(request.Description);
        group.UpdateColor(request.Color);
        if (request.FavoriteRestaurants != null)
            group.UpdateFavoriteRestaurants(request.FavoriteRestaurants);

        var updated = await groupRepository.UpdateAsync(group);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a group")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Group deleted successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Group not found")]
    public async Task<IActionResult> Delete(int id)
    {
        var group = await groupRepository.GetByIdAsync(id);
        if (group == null)
            return NotFound(new { message = "Group not found" });

        await groupRepository.DeleteAsync(id);
        return NoContent();
    }
}

public class CreateGroupRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}

public class UpdateGroupRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public List<int>? FavoriteRestaurants { get; set; }
}


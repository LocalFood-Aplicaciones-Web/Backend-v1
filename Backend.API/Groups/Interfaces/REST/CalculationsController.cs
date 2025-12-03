using System.Net.Mime;
using Backend.API.Groups.Domain.Model.Aggregates;
using Backend.API.Groups.Domain.Repositories;
using Backend.API.Groups.Domain.Services;
using Backend.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Backend.API.Groups.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Calculation endpoints")]
public class CalculationsController(
    ICalculationRepository calculationRepository,
    IColleagueRepository colleagueRepository,
    IRestaurantRepository restaurantRepository,
    IDistanceCalculationService distanceService,
    IGroupRepository groupRepository) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get all calculations for a group")]
    public async Task<IActionResult> GetAll([FromQuery] int groupId)
    {
        var calculations = await calculationRepository.GetAllByGroupIdAsync(groupId);
        return Ok(calculations);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a specific calculation by ID")]
    public async Task<IActionResult> GetById(int id)
    {
        return NotFound(new { message = "Calculation not found" });
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new calculation with distance analysis")]
    public async Task<IActionResult> Create([FromBody] CreateCalculationDto request)
    {
        var group = await groupRepository.GetByIdAsync(request.GroupId);
        if (group == null)
            return BadRequest(new { message = "Group not found" });

        var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant == null)
            return BadRequest(new { message = "Restaurant not found" });

        var colleagues = (await colleagueRepository.GetAllByGroupIdAsync(request.GroupId)).ToList();
        if (!colleagues.Any())
            return BadRequest(new { message = "Group has no colleagues" });

        var centerLat = colleagues.Average(c => c.Address?.Latitude ?? 0);
        var centerLng = colleagues.Average(c => c.Address?.Longitude ?? 0);

        var restaurantLat = restaurant.Address?.Latitude ?? 0;
        var restaurantLng = restaurant.Address?.Longitude ?? 0;

        var distanceToRestaurant = distanceService.CalculateHaversineDistance(centerLat, centerLng, restaurantLat, restaurantLng);

        var memberDistances = colleagues
            .Where(c => c.Address != null)
            .Select(c => new MemberDistance
            {
                Id = c.Id, 
                Name = c.Name,
                Distance = distanceService.CalculateHaversineDistance(c.Address!.Latitude, c.Address.Longitude, restaurantLat, restaurantLng)
            })
            .OrderBy(md => md.Distance)
            .ToList();

        var averageDistance = memberDistances.Any() ? memberDistances.Average(md => md.Distance) : 0;
        var maxSpread = memberDistances.Any() ? memberDistances.Max(md => md.Distance) - memberDistances.Min(md => md.Distance) : 0;

        var viabilityScore = CalculateViabilityScore(averageDistance, maxSpread);

        var calculation = new Calculation
        {
            GroupId = request.GroupId,
            RestaurantId = request.RestaurantId,
            RestaurantName = restaurant.Name,
            GroupMembers = colleagues.Select(c => new ColleagueInfo
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email
            }).ToList(),
            CenterPoint = new CenterPoint { Lat = centerLat, Lng = centerLng },
            Distance = Math.Round(distanceToRestaurant, 2),
            AverageDistance = Math.Round(averageDistance, 2),
            MaxSpread = Math.Round(maxSpread, 2),
            ViabilityScore = viabilityScore,
            MembersByDistance = memberDistances,
            Timestamp = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        var createdCalculation = await calculationRepository.AddAsync(calculation);
        return CreatedAtAction(nameof(GetById), new { id = createdCalculation.Id }, createdCalculation);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a calculation")]
    public async Task<IActionResult> Delete(int id)
    {
        await calculationRepository.DeleteAsync(id);
        return NoContent();
    }

    private int CalculateViabilityScore(double averageDistance, double maxSpread)
    {
        var distanceScore = Math.Max(0, 100 - (averageDistance * 10));
        var spreadScore = Math.Max(0, 100 - (maxSpread * 5));
        return (int)Math.Round((distanceScore * 0.6 + spreadScore * 0.4) / 1, MidpointRounding.AwayFromZero);
    }
}

public class CreateCalculationDto
{
    public int GroupId { get; set; }
    public int RestaurantId { get; set; }
}


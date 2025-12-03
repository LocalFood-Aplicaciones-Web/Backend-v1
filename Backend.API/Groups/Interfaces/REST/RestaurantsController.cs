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
[SwaggerTag("Available Restaurant endpoints")]
public class RestaurantsController(IRestaurantRepository restaurantRepository) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get all restaurants for the authenticated user")]
    public async Task<IActionResult> GetAll()
    {
        var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
        var restaurants = await restaurantRepository.GetAllByUserIdAsync(userId);
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a specific restaurant by ID")]
    public async Task<IActionResult> GetById(int id)
    {
        var restaurant = await restaurantRepository.GetByIdAsync(id);
        if (restaurant == null)
            return NotFound(new { message = "Restaurant not found" });
        return Ok(restaurant);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new restaurant")]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantDto request)
    {
        var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
        var restaurant = new Restaurant(request.Name, request.Cuisine, request.Rating, request.PriceRange, userId)
        {
            Phone = request.Phone,
            OpenHours = request.OpenHours
        };

        if (request.Address != null)
        {
            restaurant.Address = new Backend.API.Groups.Domain.Model.ValueObjects.Address(
                request.Address.Street,
                request.Address.City,
                request.Address.Latitude,
                request.Address.Longitude
            );
        }

        var createdRestaurant = await restaurantRepository.AddAsync(restaurant);
        return CreatedAtAction(nameof(GetById), new { id = createdRestaurant.Id }, createdRestaurant);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an existing restaurant")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRestaurantDto request)
    {
        var restaurant = await restaurantRepository.GetByIdAsync(id);
        if (restaurant == null)
            return NotFound(new { message = "Restaurant not found" });

        restaurant.UpdateName(request.Name);
        restaurant.UpdateRating(request.Rating);
        restaurant.Phone = request.Phone;
        restaurant.OpenHours = request.OpenHours;

        if (request.Address != null)
        {
            restaurant.UpdateAddress(new Backend.API.Groups.Domain.Model.ValueObjects.Address(
                request.Address.Street,
                request.Address.City,
                request.Address.Latitude,
                request.Address.Longitude
            ));
        }

        var updated = await restaurantRepository.UpdateAsync(restaurant);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a restaurant")]
    public async Task<IActionResult> Delete(int id)
    {
        var restaurant = await restaurantRepository.GetByIdAsync(id);
        if (restaurant == null)
            return NotFound(new { message = "Restaurant not found" });

        await restaurantRepository.DeleteAsync(id);
        return NoContent();
    }
}

public class CreateRestaurantDto
{
    public string Name { get; set; } = string.Empty;
    public string Cuisine { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string PriceRange { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string OpenHours { get; set; } = string.Empty;
    public AddressDto? Address { get; set; }
}

public class UpdateRestaurantDto
{
    public string Name { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string OpenHours { get; set; } = string.Empty;
    public AddressDto? Address { get; set; }
}


namespace Backend.API.Restaurants.Domain.Model.ValueObjects;

public record RestaurantLocation(
    string Address,
    string City,
    string Country
);
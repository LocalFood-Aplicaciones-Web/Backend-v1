namespace Backend.API.Restaurants.Domain.Model.Aggregates;

using Backend.API.Restaurants.Domain.Model.Entities;

public class RestaurantAggregate
{
    public Restaurant Restaurant { get; }

    public RestaurantAggregate(Restaurant restaurant)
    {
        Restaurant = restaurant;
    }
}
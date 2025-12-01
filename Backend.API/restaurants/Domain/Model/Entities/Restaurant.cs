namespace Backend.API.Restaurants.Domain.Model.Entities;

using Backend.API.Restaurants.Domain.Model.ValueObjects;

public class Restaurant
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string ImageUrl { get; private set; }
    public double Rating { get; private set; }

    public List<RestaurantLocation> Locations { get; private set; }

    private Restaurant() {}

    public Restaurant(string name, string imageUrl, double rating)
    {
        Id = Guid.NewGuid();
        Name = name;
        ImageUrl = imageUrl;
        Rating = rating;
        Locations = new List<RestaurantLocation>();
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Restaurant name cannot be empty.");

        Name = newName;
    }

    public void AddLocation(RestaurantLocation location)
    {
        Locations.Add(location);
    }
}
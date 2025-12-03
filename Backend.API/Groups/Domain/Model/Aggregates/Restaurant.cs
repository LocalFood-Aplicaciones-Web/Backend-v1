using Backend.API.Groups.Domain.Model.ValueObjects;

namespace Backend.API.Groups.Domain.Model.Aggregates;

public class Restaurant(string name, string cuisine, double rating, string priceRange, int userId)
{
    public Restaurant() : this(string.Empty, string.Empty, 0, string.Empty, 0)
    {
    }

    public int Id { get; set; }
    public string Name { get; set; } = name;
    public string Cuisine { get; set; } = cuisine;
    public double Rating { get; set; } = rating;
    public string PriceRange { get; set; } = priceRange;
    public Address? Address { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string OpenHours { get; set; } = string.Empty;
    public int UserId { get; set; } = userId;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Restaurant UpdateName(string newName)
    {
        Name = newName;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }

    public Restaurant UpdateRating(double newRating)
    {
        Rating = newRating;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }

    public Restaurant UpdateAddress(Address newAddress)
    {
        Address = newAddress;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }
}


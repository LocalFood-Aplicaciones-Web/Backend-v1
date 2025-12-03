using System.Text.Json.Serialization;

namespace Backend.API.Groups.Domain.Model.Aggregates;

public class Group(string name, string description, string color, int userId)
{
    public Group() : this(string.Empty, string.Empty, string.Empty, 0)
    {
    }

    public int Id { get; set; }
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public string Color { get; set; } = color;
    public int UserId { get; set; } = userId;
    public List<int> FavoriteRestaurants { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Group UpdateName(string newName)
    {
        Name = newName;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }

    public Group UpdateDescription(string newDescription)
    {
        Description = newDescription;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }

    public Group UpdateColor(string newColor)
    {
        Color = newColor;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }

    public Group UpdateFavoriteRestaurants(List<int> restaurantIds)
    {
        FavoriteRestaurants = restaurantIds;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }
}


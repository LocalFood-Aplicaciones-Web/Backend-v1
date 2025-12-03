using Backend.API.Groups.Domain.Model.ValueObjects;

namespace Backend.API.Groups.Domain.Model.Aggregates;

public class Calculation
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; } = string.Empty;
    public List<ColleagueInfo> GroupMembers { get; set; } = new();
    public CenterPoint CenterPoint { get; set; } = new();
    public double Distance { get; set; }
    public double AverageDistance { get; set; }
    public double MaxSpread { get; set; }
    public int ViabilityScore { get; set; }
    public List<MemberDistance> MembersByDistance { get; set; } = new();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class ColleagueInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class CenterPoint
{
    public double Lat { get; set; }
    public double Lng { get; set; }
}

public class MemberDistance
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Distance { get; set; }
}


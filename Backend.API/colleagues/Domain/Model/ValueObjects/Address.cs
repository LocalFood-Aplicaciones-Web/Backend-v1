namespace Backend.API.colleagues.Domain.Model.ValueObjects;

/// <summary>
///     Represents an address value object.
/// </summary>
public class Address
{
    public Address()
    {
        Street = string.Empty;
        City = string.Empty;
        Latitude = 0;
        Longitude = 0;
    }

    public Address(string street, string city, double latitude, double longitude)
    {
        Street = street;
        City = city;
        Latitude = latitude;
        Longitude = longitude;
    }

    public string Street { get; private set; }
    public string City { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    /// <summary>
    /// Returns the full address as a string representation.
    /// </summary>
    public override string ToString()
    {
        return $"{Street}, {City}";
    }
}
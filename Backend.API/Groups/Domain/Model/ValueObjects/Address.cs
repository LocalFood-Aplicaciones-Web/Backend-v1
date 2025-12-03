namespace Backend.API.Groups.Domain.Model.ValueObjects;

public class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public Address()
    {
    }

    public Address(string street, string city, double latitude, double longitude)
    {
        Street = street;
        City = city;
        Latitude = latitude;
        Longitude = longitude;
    }
}


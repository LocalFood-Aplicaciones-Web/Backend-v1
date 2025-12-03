namespace Backend.API.Groups.Domain.Services;

public interface IDistanceCalculationService
{
    double CalculateHaversineDistance(double lat1, double lon1, double lat2, double lon2);
}

public class DistanceCalculationService : IDistanceCalculationService
{
    private const double EarthRadiusKm = 6371.0;

    public double CalculateHaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        var lat1Rad = ToRadians(lat1);
        var lon1Rad = ToRadians(lon1);
        var lat2Rad = ToRadians(lat2);
        var lon2Rad = ToRadians(lon2);

        var dlat = lat2Rad - lat1Rad;
        var dlon = lon2Rad - lon1Rad;

        var a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin(dlon / 2) * Math.Sin(dlon / 2);

        var c = 2 * Math.Asin(Math.Sqrt(a));
        return EarthRadiusKm * c;
    }

    private double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
}


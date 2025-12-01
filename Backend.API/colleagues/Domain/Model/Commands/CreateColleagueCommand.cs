using Backend.API.colleagues.Domain.Model.ValueObjects;

namespace Backend.API.colleagues.Domain.Model.Commands;

/// <summary>
///     Command to create a new colleague.
/// </summary>
/// <param name="Name">The name of the colleague.</param>
/// <param name="Email">The email of the colleague.</param>
/// <param name="Phone">The phone number.</param>
/// <param name="Street">Address street.</param>
/// <param name="City">Address city.</param>
/// <param name="Latitude">Address latitude.</param>
/// <param name="Longitude">Address longitude.</param>
/// <param name="GroupId">Optional Group ID assignment.</param>
/// <param name="UserId">The ID of the user creating the colleague (Tenant).</param>
public record CreateColleagueCommand(
    string Name, 
    string Email, 
    string Phone, 
    string Street, 
    string City, 
    double Latitude, 
    double Longitude, 
    int? GroupId, 
    int UserId
);
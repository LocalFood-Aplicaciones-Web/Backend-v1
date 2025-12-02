namespace Backend.API.Colleagues.Domain.Model.Commands;

/// <summary>
///     Command to update an existing colleague's profile.
/// </summary>
/// <param name="Id">The ID of the colleague to update.</param>
/// <param name="Name">The new name.</param>
/// <param name="Email">The new email.</param>
/// <param name="Phone">The new phone.</param>
/// <param name="Street">New address street.</param>
/// <param name="City">New address city.</param>
/// <param name="Latitude">New latitude.</param>
/// <param name="Longitude">New longitude.</param>
/// <param name="GroupId">New group assignment (optional).</param>
public record UpdateColleagueCommand(
    int Id,
    string Name, 
    string Email, 
    string Phone, 
    string Street, 
    string City, 
    double Latitude, 
    double Longitude, 
    int? GroupId
);
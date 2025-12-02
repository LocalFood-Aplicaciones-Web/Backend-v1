namespace Backend.API.Colleagues.Domain.Model.Commands;

/// <summary>
///     Command to create a new group of colleagues.
/// </summary>
/// <param name="Name">The name of the group.</param>
/// <param name="Description">A brief description of the group.</param>
/// <param name="Color">Hex color code for the group tag.</param>
/// <param name="UserId">The ID of the user creating the group.</param>
public record CreateGroupCommand(
    string Name, 
    string Description, 
    string Color, 
    int UserId
);
namespace Backend.API.colleagues.Domain.Model.Commands;

/// <summary>
///     Command to update an existing group.
/// </summary>
/// <param name="Id">The ID of the group to update.</param>
/// <param name="Name">The new name.</param>
/// <param name="Description">The new description.</param>
/// <param name="Color">The new color.</param>
public record UpdateGroupCommand(
    int Id,
    string Name, 
    string Description, 
    string Color
);
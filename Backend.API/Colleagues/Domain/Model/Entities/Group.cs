
using Backend.API.Colleagues.Domain.Model.Commands;

namespace Backend.API.colleagues.Domain.Model.Entities;

/// <summary>
///     Represents a group of colleagues in the ACME Learning Center Platform.
/// </summary>
public partial class Group
{
    /// <summary>
    ///     Default constructor for the group entity
    /// </summary>
    public Group()
    {
        Name = string.Empty;
        Description = string.Empty;
        Color = string.Empty;
    }

    /// <summary>
    ///     Constructor for the group entity
    /// </summary>
    /// <param name="name">The name of the group</param>
    /// <param name="description">The description of the group</param>
    /// <param name="color">The color identifier for the group</param>
    /// <param name="userId">The user id owner of the group</param>
    public Group(string name, string description, string color, int userId)
    {
        Name = name;
        Description = description;
        Color = color;
        UserId = userId;
    }

    /// <summary>
    ///     Constructor from command
    /// </summary>
    /// <param name="command">The create group command</param>
    public Group(CreateGroupCommand command)
    {
        Name = command.Name;
        Description = command.Description;
        Color = command.Color;
        UserId = command.UserId;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public int UserId { get; set; }
}
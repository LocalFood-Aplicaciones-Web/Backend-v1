using Backend.API.Colleagues.Domain.Model.ValueObjects;
using Backend.API.Colleagues.Domain.Model.Entities;
namespace Backend.API.Colleagues.Domain.Model.Aggregate;

/// <summary>
///     Colleague aggregate root entity.
/// </summary>
public partial class Colleague
{
    public Colleague()
    {
        Name = string.Empty;
        Email = string.Empty;
        Phone = string.Empty;
        Address = new Address();
    }

    public Colleague(string name, string email, string phone, Address address, int userId) : this()
    {
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
        UserId = userId;
    }

    public int Id { get; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    
    // Value Object embebido
    public Address Address { get; private set; }

    // Relación con Grupo (Opcional, puede ser null si está "Unassigned")
    public int? GroupId { get; private set; }
    public Group? Group { get; internal set; }

    // Tenant / Dueño del registro
    public int UserId { get; private set; }

    /// <summary>
    ///     Updates the colleague information.
    /// </summary>
    public void UpdateProfile(string name, string email, string phone, Address address)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
    }

    /// <summary>
    ///     Assigns the colleague to a specific group.
    /// </summary>
    public void AssignToGroup(int groupId)
    {
        GroupId = groupId;
    }

    /// <summary>
    ///     Removes the colleague from their current group.
    /// </summary>
    public void UnassignGroup()
    {
        GroupId = null;
    }
}
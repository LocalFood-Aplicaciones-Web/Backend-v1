using Backend.API.Groups.Domain.Model.ValueObjects;

namespace Backend.API.Groups.Domain.Model.Aggregates;

public class Colleague(string name, string email, string phone, int userId, int? groupId = null)
{
    public Colleague() : this(string.Empty, string.Empty, string.Empty, 0)
    {
    }

    public int Id { get; set; }
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string Phone { get; set; } = phone;
    public int UserId { get; set; } = userId;
    public int? GroupId { get; set; } = groupId;
    public bool IsLeader { get; set; }
    public Address? Address { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Colleague UpdateName(string newName)
    {
        Name = newName;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }

    public Colleague UpdateEmail(string newEmail)
    {
        Email = newEmail;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }

    public Colleague UpdatePhone(string newPhone)
    {
        Phone = newPhone;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }

    public Colleague UpdateAddress(Address newAddress)
    {
        Address = newAddress;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }

    public Colleague UpdateLeaderStatus(bool isLeader)
    {
        IsLeader = isLeader;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }
}


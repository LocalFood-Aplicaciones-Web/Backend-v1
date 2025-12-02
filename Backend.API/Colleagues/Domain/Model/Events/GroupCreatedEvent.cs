using Backend.API.Shared.Domain.Model.Events;
using Backend.API.Colleagues.Domain.Model.Entities;

namespace Backend.API.Colleagues.Domain.Model.Events;

/// <summary>
///     Event triggered when a new group is created.
/// </summary>
/// <param name="group">The group that was created.</param>
public class GroupCreatedEvent(Group group) : IEvent
{
    public Group Group { get; } = group;
}
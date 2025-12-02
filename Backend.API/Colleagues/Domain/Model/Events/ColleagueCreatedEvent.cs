using Backend.API.Shared.Domain.Model.Events;
using Backend.API.Colleagues.Domain.Model.Aggregate;

namespace Backend.API.Colleagues.Domain.Model.Events;

/// <summary>
///     Event triggered when a new colleague is created.
/// </summary>
/// <param name="colleague">The colleague that was created.</param>
public class ColleagueCreatedEvent(Colleague colleague) : IEvent
{
    public Colleague Colleague { get; } = colleague;
}
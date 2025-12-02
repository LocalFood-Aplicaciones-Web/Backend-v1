namespace Backend.API.Colleagues.Domain.Model.Queries;

/// <summary>
///     Represents a query to get all colleagues belonging to a specific group.
/// </summary>
/// <param name="GroupId">The id of the group to filter colleagues</param>
public record GetAllColleaguesByGroupIdQuery(int GroupId);
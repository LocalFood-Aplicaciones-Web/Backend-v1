namespace Backend.API.Colleagues.Domain.Model.Queries;

/// <summary>
///     Represents a query to get a group by id.
/// </summary>
/// <param name="GroupId">The id of the group to get</param>
public record GetGroupByIdQuery(int GroupId);
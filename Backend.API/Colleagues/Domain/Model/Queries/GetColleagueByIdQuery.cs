namespace Backend.API.Colleagues.Domain.Model.Queries;

/// <summary>
///     Represents a query to get a colleague by id.
/// </summary>
/// <param name="ColleagueId">The id of the colleague to get</param>
public record GetColleagueByIdQuery(int ColleagueId);
using Backend.API.Groups.Domain.Model.Aggregates;

namespace Backend.API.Groups.Domain.Repositories;

public interface IColleagueRepository
{
    Task<Colleague?> GetByIdAsync(int id);
    Task<IEnumerable<Colleague>> GetAllByUserIdAsync(int userId);
    Task<IEnumerable<Colleague>> GetAllByGroupIdAsync(int groupId);
    Task<Colleague> AddAsync(Colleague colleague);
    Task<Colleague> UpdateAsync(Colleague colleague);
    Task DeleteAsync(int id);
}


using Backend.API.Groups.Domain.Model.Aggregates;

namespace Backend.API.Groups.Domain.Repositories;

public interface IGroupRepository
{
    Task<Group?> GetByIdAsync(int id);
    Task<IEnumerable<Group>> GetAllByUserIdAsync(int userId);
    Task<Group> AddAsync(Group group);
    Task<Group> UpdateAsync(Group group);
    Task DeleteAsync(int id);
}


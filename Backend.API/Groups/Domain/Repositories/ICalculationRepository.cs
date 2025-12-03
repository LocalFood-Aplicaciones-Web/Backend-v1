using Backend.API.Groups.Domain.Model.Aggregates;

namespace Backend.API.Groups.Domain.Repositories;

public interface ICalculationRepository
{
    Task<Calculation?> GetByIdAsync(int id);
    Task<IEnumerable<Calculation>> GetAllByGroupIdAsync(int groupId);
    Task<Calculation> AddAsync(Calculation calculation);
    Task DeleteAsync(int id);
}


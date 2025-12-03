using Backend.API.Groups.Domain.Model.Aggregates;

namespace Backend.API.Groups.Domain.Repositories;

public interface IRestaurantRepository
{
    Task<Restaurant?> GetByIdAsync(int id);
    Task<IEnumerable<Restaurant>> GetAllByUserIdAsync(int userId);
    Task<Restaurant> AddAsync(Restaurant restaurant);
    Task<Restaurant> UpdateAsync(Restaurant restaurant);
    Task DeleteAsync(int id);
}


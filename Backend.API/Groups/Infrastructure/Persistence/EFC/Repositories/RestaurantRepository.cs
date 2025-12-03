using Backend.API.Groups.Domain.Model.Aggregates;
using Backend.API.Groups.Domain.Repositories;
using Backend.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Backend.API.Groups.Infrastructure.Persistence.EFC.Repositories;

public class RestaurantRepository(AppDbContext context) : IRestaurantRepository
{
    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        return await context.Restaurants.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Restaurant>> GetAllByUserIdAsync(int userId)
    {
        return await context.Restaurants.Where(r => r.UserId == userId).ToListAsync();
    }

    public async Task<Restaurant> AddAsync(Restaurant restaurant)
    {
        context.Restaurants.Add(restaurant);
        await context.SaveChangesAsync();
        return restaurant;
    }

    public async Task<Restaurant> UpdateAsync(Restaurant restaurant)
    {
        context.Restaurants.Update(restaurant);
        await context.SaveChangesAsync();
        return restaurant;
    }

    public async Task DeleteAsync(int id)
    {
        var restaurant = await context.Restaurants.FindAsync(id);
        if (restaurant != null)
        {
            context.Restaurants.Remove(restaurant);
            await context.SaveChangesAsync();
        }
    }
}


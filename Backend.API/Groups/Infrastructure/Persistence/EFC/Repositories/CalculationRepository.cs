using Backend.API.Groups.Domain.Model.Aggregates;
using Backend.API.Groups.Domain.Repositories;
using Backend.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Backend.API.Groups.Infrastructure.Persistence.EFC.Repositories;

public class CalculationRepository(AppDbContext context) : ICalculationRepository
{
    public async Task<Calculation?> GetByIdAsync(int id)
    {
        return await context.Calculations.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Calculation>> GetAllByGroupIdAsync(int groupId)
    {
        return await context.Calculations.Where(c => c.GroupId == groupId).ToListAsync();
    }

    public async Task<Calculation> AddAsync(Calculation calculation)
    {
        context.Calculations.Add(calculation);
        await context.SaveChangesAsync();
        return calculation;
    }

    public async Task DeleteAsync(int id)
    {
        var calculation = await context.Calculations.FindAsync(id);
        if (calculation != null)
        {
            context.Calculations.Remove(calculation);
            await context.SaveChangesAsync();
        }
    }
}


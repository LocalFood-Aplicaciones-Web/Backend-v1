using Backend.API.Groups.Domain.Model.Aggregates;
using Backend.API.Groups.Domain.Repositories;
using Backend.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Backend.API.Groups.Infrastructure.Persistence.EFC.Repositories;

public class ColleagueRepository(AppDbContext context) : IColleagueRepository
{
    public async Task<Colleague?> GetByIdAsync(int id)
    {
        return await context.Colleagues.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Colleague>> GetAllByUserIdAsync(int userId)
    {
        return await context.Colleagues.Where(c => c.UserId == userId).ToListAsync();
    }

    public async Task<IEnumerable<Colleague>> GetAllByGroupIdAsync(int groupId)
    {
        return await context.Colleagues.Where(c => c.GroupId == groupId).ToListAsync();
    }

    public async Task<Colleague> AddAsync(Colleague colleague)
    {
        context.Colleagues.Add(colleague);
        await context.SaveChangesAsync();
        return colleague;
    }

    public async Task<Colleague> UpdateAsync(Colleague colleague)
    {
        context.Colleagues.Update(colleague);
        await context.SaveChangesAsync();
        return colleague;
    }

    public async Task DeleteAsync(int id)
    {
        var colleague = await context.Colleagues.FindAsync(id);
        if (colleague != null)
        {
            context.Colleagues.Remove(colleague);
            await context.SaveChangesAsync();
        }
    }
}


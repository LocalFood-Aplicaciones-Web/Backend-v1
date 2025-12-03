using Backend.API.Groups.Domain.Model.Aggregates;
using Backend.API.Groups.Domain.Repositories;
using Backend.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Backend.API.Groups.Infrastructure.Persistence.EFC.Repositories;

public class GroupRepository(AppDbContext context) : IGroupRepository
{
    public async Task<Group?> GetByIdAsync(int id)
    {
        return await context.Groups.FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<Group>> GetAllByUserIdAsync(int userId)
    {
        return await context.Groups.Where(g => g.UserId == userId).ToListAsync();
    }

    public async Task<Group> AddAsync(Group group)
    {
        context.Groups.Add(group);
        await context.SaveChangesAsync();
        return group;
    }

    public async Task<Group> UpdateAsync(Group group)
    {
        context.Groups.Update(group);
        await context.SaveChangesAsync();
        return group;
    }

    public async Task DeleteAsync(int id)
    {
        var group = await context.Groups.FindAsync(id);
        if (group != null)
        {
            context.Groups.Remove(group);
            await context.SaveChangesAsync();
        }
    }
}


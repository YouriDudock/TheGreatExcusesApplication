using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TheGreatExcusesApplication.Domain.Entities;

namespace TheGreatExcusesApplication.Infrastructure.Repositories;

public class ExcuseRepository(ApplicationDbContext context) : IExcuseRepository
{
    public async Task<IEnumerable<Excuse>> GetAsync(Expression<Func<Excuse, bool>> predicate)
    {
        return await context.Excuses
            .Include(e => e.Score)
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<Excuse?> GetByIdAsync(int id)
    {
        return await context.Excuses
            .Include(e => e.Score)
            .FirstAsync(e => e.Id == id);
    }

    public async Task<Excuse> UpdateExcuse(Excuse excuse)
    {
        var updatedExcuse = context.Excuses.Update(excuse);
        await context.SaveChangesAsync();
        return updatedExcuse.Entity;
    }
}
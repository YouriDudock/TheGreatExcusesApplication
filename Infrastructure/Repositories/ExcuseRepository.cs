using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TheGreatExcusesApplication.Domain.Entities;

namespace TheGreatExcusesApplication.Infrastructure.Repositories;

public class ExcuseRepository(ApplicationDbContext context) : IExcuseRepository
{
    public async Task<IEnumerable<Excuse>> GetAsync(Expression<Func<Excuse, bool>> predicate)
    {
        return await context.Excuses.Where(predicate).ToListAsync();
    }
}
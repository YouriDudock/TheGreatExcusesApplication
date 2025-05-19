using System.Linq.Expressions;
using TheGreatExcusesApplication.Domain.Entities;

namespace TheGreatExcusesApplication.Infrastructure.Repositories;

public interface IExcuseRepository
{
    public Task<IEnumerable<Excuse>> GetAsync(Expression<Func<Excuse, bool>> predicate);
}
using TheGreatExcusesApplication.Domain;
using TheGreatExcusesApplication.Domain.Entities;

namespace TheGreatExcusesApplication.Application.Services;

public interface IExcuseService
{
    public Task<Excuse> FindExcuse(ExcuseCategory category);
    
    public Task<Excuse> RegisterExcuseScore(int excuseId, bool succeeded);
}
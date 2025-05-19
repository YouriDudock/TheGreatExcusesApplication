using TheGreatExcusesApplication.Domain;
using TheGreatExcusesApplication.Domain.Entities;

namespace TheGreatExcusesApplication.Application.Services;

public interface IExcuseService
{
    public Task<Excuse> FindExcuse(ExcuseCategory category);
}
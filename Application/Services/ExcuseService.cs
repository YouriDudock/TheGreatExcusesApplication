using System.Collections.Immutable;
using TheGreatExcusesApplication.Application.Providers;
using TheGreatExcusesApplication.Domain;
using TheGreatExcusesApplication.Domain.Entities;
using TheGreatExcusesApplication.Infrastructure.Repositories;

namespace TheGreatExcusesApplication.Application.Services;

public class ExcuseService(
    IExcuseRepository excuseRepository, 
    IRandomProvider randomProvider
    ) : IExcuseService
{
    public async Task<Excuse> FindExcuse(ExcuseCategory category)
    {
        var excuses = (await excuseRepository.GetAsync(excuse => excuse.Category == category)).ToImmutableList();

        if (excuses.IsEmpty)
        {
            throw new NotSupportedException($"Excuse category not found: {category}");
        }

        var randomExcuse = excuses[randomProvider.Next(excuses.Count)];
        return randomExcuse;
    }
}
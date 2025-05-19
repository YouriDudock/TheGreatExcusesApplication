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

    public async Task<Excuse> RegisterExcuseScore(int excuseId, bool succeeded)
    {
        var excuse = await excuseRepository.GetByIdAsync(excuseId);

        if (excuse == null)
        { 
            throw new KeyNotFoundException($"Excuse not found with id {excuseId}");
        }

        // Add or substract from score
        // Todo -> integer overflow/underflow handling
        excuse.Score.Value += succeeded ? 1 : -1;
        
        return await excuseRepository.UpdateExcuse(excuse);
    }
}
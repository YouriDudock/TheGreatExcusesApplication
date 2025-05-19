namespace TheGreatExcusesApplication.Application.Providers;

public class RandomProvider : IRandomProvider
{
    private readonly Random _random = new();

    public int Next(int maxValue)
        => _random.Next(maxValue);
}
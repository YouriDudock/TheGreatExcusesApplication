namespace TheGreatExcusesApplication.Application.Providers;

public interface IRandomProvider
{
    public int Next(int maxValue); // Like Random.Next()
}
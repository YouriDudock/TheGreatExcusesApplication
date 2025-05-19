using System.Linq.Expressions;
using Moq;
using TheGreatExcusesApplication.Application.Providers;
using TheGreatExcusesApplication.Application.Services;
using TheGreatExcusesApplication.Domain;
using TheGreatExcusesApplication.Domain.Entities;
using TheGreatExcusesApplication.Infrastructure.Repositories;
using Xunit;

namespace TheGreatExcusesApplication.Tests.Services;

public class ExcuseServiceTests
{
    [Fact]
    public async Task FindExcuse_ShouldReturnCorrectExcuse()
    {
        // Arrange 
        var repo = new Mock<IExcuseRepository>();
        var randomProvider = new Mock<RandomProvider>();

        var mockExcuse = CreateExcuse(0, "Bridge was open", ExcuseCategory.Work);

        repo.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Excuse, bool>>>()))
            .ReturnsAsync(new List<Excuse> { mockExcuse });

        var service = new ExcuseService(repo.Object, randomProvider.Object);

        // Act
        var result = await service.FindExcuse(mockExcuse.Category);

        // Assert
        Assert.Equal(mockExcuse, result);
    }

    [Fact]
    public async Task FindExcuse_ShouldReturnCorrectExcuse_WhenRandomSelected()
    {
        // Arrange 
        var repo = new Mock<IExcuseRepository>();
        var randomProvider = new Mock<IRandomProvider>();

        var mockExcuse = CreateExcuse(1, "Train got hijacked", ExcuseCategory.Work);

        var excuses = new List<Excuse>
        {
            CreateExcuse(0, "Bridge was open", ExcuseCategory.Work),
            mockExcuse,
            CreateExcuse(2, "Forgot to drink coffee at home", ExcuseCategory.Work)
        };

        repo.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Excuse, bool>>>()))
            .ReturnsAsync(excuses);

        randomProvider.Setup(r => r.Next(It.IsAny<int>()))
            .Returns(excuses.FindIndex(e => e.Id == mockExcuse.Id));

        var service = new ExcuseService(repo.Object, randomProvider.Object);

        // Act
        var result = await service.FindExcuse(mockExcuse.Category);

        // Assert
        Assert.Equal(mockExcuse, result);
    }

    [Fact]
    public async Task FindExcuse_ShouldThrowNotSupportedException_WhenNoExcuseFound()
    {
        // Arrange 
        var repo = new Mock<IExcuseRepository>();
        var randomProvider = new Mock<RandomProvider>();

        repo.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Excuse, bool>>>()))
            // Return an empty result
            .ReturnsAsync(new List<Excuse>());

        var service = new ExcuseService(repo.Object, randomProvider.Object);

        // Assert
        await Assert.ThrowsAsync<NotSupportedException>(() => service.FindExcuse(ExcuseCategory.Work));
    }

    [Fact]
    public async Task RegisterExcuseScore_ShouldIncreaseScore_WhenSucceeded()
    {
        // Arrange 
        var repo = new Mock<IExcuseRepository>();
        var randomProvider = new Mock<RandomProvider>();

        const int startingScore = 0;  
        const bool succeeded = true; // excuse was accepted

        var mockExcuse = CreateExcuse(0, "Bridge was open", ExcuseCategory.Work, startingScore);

        repo.Setup(r => r.GetByIdAsync(mockExcuse.Id))
            .ReturnsAsync(mockExcuse);

        repo.Setup(r => r.UpdateExcuse(mockExcuse))
            .ReturnsAsync(mockExcuse);

        var service = new ExcuseService(repo.Object, randomProvider.Object);

        // Act
        var result = await service.RegisterExcuseScore(mockExcuse.Id, succeeded);

        // Assert
        Assert.Equal(startingScore + 1, mockExcuse.Score.Value);
    }
    
    [Fact]
    public async Task RegisterExcuseScore_ShouldDecreaseScore_WhenFailed()
    {
        // Arrange 
        var repo = new Mock<IExcuseRepository>();
        var randomProvider = new Mock<RandomProvider>();

        const int startingScore = 0;    
        const bool succeeded = false; // excuse was not accepted

        var mockExcuse = CreateExcuse(0, "Bridge was open", ExcuseCategory.Work, startingScore);

        repo.Setup(r => r.GetByIdAsync(mockExcuse.Id))
            .ReturnsAsync(mockExcuse);

        repo.Setup(r => r.UpdateExcuse(mockExcuse))
            .ReturnsAsync(mockExcuse);

        var service = new ExcuseService(repo.Object, randomProvider.Object);

        // Act
        var result = await service.RegisterExcuseScore(mockExcuse.Id, succeeded);

        // Assert
        Assert.Equal(startingScore - 1, mockExcuse.Score.Value);
    }
    
    [Fact]
    public async Task RegisterExcuseScore_ShouldThrowException_WhenExcuseNotFound()
    {
        // Arrange 
        var repo = new Mock<IExcuseRepository>();
        var randomProvider = new Mock<RandomProvider>();

        const bool succeeded = false; // excuse was not accepted

        Excuse? mockExcuse = null;

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(mockExcuse);

        var service = new ExcuseService(repo.Object, randomProvider.Object);
        
        // Assert
       await Assert.ThrowsAsync<KeyNotFoundException>(() => service.RegisterExcuseScore(0, succeeded));
    }

    private Excuse CreateExcuse(int id, string text, ExcuseCategory category, int scoreValue = 0)
    {
        return new Excuse()
        {
            Id = id, Text = text, Category = category, CreatedDate = DateTime.Now,
            Score = new Score
            {
                ExcuseId = id,
                Id = 0,
                Value = scoreValue
            }
        };
    }
}
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

    private Excuse CreateExcuse(int id, string text, ExcuseCategory category)
    {
        return new Excuse() {Id = id, Text = text, Category = category, CreatedDate = DateTime.Now};
    }
}
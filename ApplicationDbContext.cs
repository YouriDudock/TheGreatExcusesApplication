using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using TheGreatExcusesApplication.Domain.Entities;

namespace TheGreatExcusesApplication;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Excuse> Excuses { get; set; }
}
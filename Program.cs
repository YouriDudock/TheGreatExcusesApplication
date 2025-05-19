using Microsoft.EntityFrameworkCore;
using TheGreatExcusesApplication;
using TheGreatExcusesApplication.Application.Providers;
using TheGreatExcusesApplication.Application.Services;
using TheGreatExcusesApplication.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Repos DI
builder.Services.AddScoped<IExcuseRepository, ExcuseRepository>();

// Services DI
builder.Services.AddScoped<IExcuseService, ExcuseService>();

// Providers DI
builder.Services.AddScoped<IRandomProvider, RandomProvider>();

var app = builder.Build();

app.MapControllerRoute(
    name: "default", // Route name
    pattern: "api/excuse/{action=Get}/{id?}", // Pattern for the route
    defaults: new { controller = "Excuse" } // Controller to use
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
using Application;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// Add API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply migrations and seed data
if (app.Environment.IsDevelopment() ||
    builder.Configuration.GetValue<bool>("EF:MigrateOnStartup"))
{
    using var scope = app.Services.CreateScope();
    var dbCtx = scope.ServiceProvider.GetRequiredService<AppDb>();
    dbCtx.Database.Migrate();

    // Seed initial data if empty
    if (!dbCtx.Todos.Any())
    {
        dbCtx.Todos.AddRange(
            new Todo { Title = "牛乳を買う", Done = false },
            new Todo { Title = "本を読む", Done = false },
            new Todo { Title = "筋トレする", Done = true });
        dbCtx.SaveChanges();
    }
}

// Map endpoints
app.MapGet("/hello", () => new { message = "Hello World!" });
app.MapTodoEndpoints();

app.Run();

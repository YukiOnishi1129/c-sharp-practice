using Application.Interfaces;
using Application.Services;
using Infrastructure.Repositories;
using Web.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITodoRepository, InMemoryTodoRepository>(); 
builder.Services.AddScoped<TodoService>();

var app = builder.Build();


app.MapTodoEndpoints();

app.Run();
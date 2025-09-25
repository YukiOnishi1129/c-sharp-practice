var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/hello", () => new { Message = "Hello, API!" });

app.MapGet("/hello/{name}", (string name) => new { Message = $"Hello, {name}!" });

app.MapPost("/greet", (User user) => new { message = $"Hello, {user.Name}. You are {user.Age} years old." });

app.Run();

record User(string Name, int Age);
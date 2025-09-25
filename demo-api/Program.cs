var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/hello", () => new { Message = "Hello, API!" });

app.MapGet("/hello/{name}", (string name) => new { Message = $"Hello, {name}!" });

app.Run();
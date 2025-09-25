var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/hello", () => new { Message = "Hello, API!" });

app.Run();
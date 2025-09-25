var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// /hello にアクセスしたら JSON を返す
app.MapGet("/hello", () => new { message = "Hello World!" });

app.Run();
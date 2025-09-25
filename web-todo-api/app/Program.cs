using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

string? configured = builder.Configuration.GetConnectionString("Db");

string host = Environment.GetEnvironmentVariable("POSTGRES_CONTAINER_NAME") ?? "db";
string port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
string? user = Environment.GetEnvironmentVariable("POSTGRES_USER");
string? pwd  = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
string? db = Environment.GetEnvironmentVariable("POSTGRES_DB");


bool runningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

if (!runningInContainer && string.IsNullOrWhiteSpace(user) && string.IsNullOrWhiteSpace(pwd) && string.IsNullOrWhiteSpace(db))
{
	host = "localhost";
	user ??= "user";
	pwd ??= "password";
	db ??= "todo_db";
}

string fromEnv = $"Host={host};Port={port};Username={user ?? "app"};Password={pwd ?? "app"};Database={db ?? "todo"}";
string conn = !string.IsNullOrWhiteSpace(configured) ? configured! : fromEnv;

// 2) DbContext 登録
builder.Services.AddDbContext<AppDb>(opt => opt.UseNpgsql(conn));

var app = builder.Build();

// 3) 起動時マイグレーション（開発向け）
if (app.Environment.IsDevelopment() ||
	builder.Configuration.GetValue<bool>("EF:MigrateOnStartup"))
{
	using var scope = app.Services.CreateScope();
	var dbCtx = scope.ServiceProvider.GetRequiredService<AppDb>();
	dbCtx.Database.Migrate();
	
	 // 何か1件でもあればスキップ
        if (!dbCtx.Todos.Any())
        {
            dbCtx.Todos.AddRange(
                new Todo { Title = "牛乳を買う", Done = false },
                new Todo { Title = "本を読む", Done = false },
                new Todo { Title = "筋トレする", Done = true }
            );
            dbCtx.SaveChanges();
        }
}
// 4) 最小エンドポイント
app.MapGet("/hello", () => new { message = "Hello World!" });

app.MapGet("/todos", async (
    AppDb db,
    bool? done,
    string? q,
    int? skip,
    int? take,
    CancellationToken ct) =>
{
    var query = db.Todos.AsNoTracking().AsQueryable();

    if (done.HasValue) query = query.Where(t => t.Done == done.Value);
    if (!string.IsNullOrWhiteSpace(q))
        // ILIKE で大文字小文字無視（Postgres）
        query = query.Where(t => EF.Functions.ILike(t.Title, $"%{q}%"));

    query = query.OrderBy(t => t.Title);

    if (skip is > 0) query = query.Skip(skip.Value);
    if (take is > 0) query = query.Take(Math.Min(take.Value, 100)); // 上限100

    var items = await query.ToListAsync(ct);
    return Results.Ok(items);
});


app.Run();
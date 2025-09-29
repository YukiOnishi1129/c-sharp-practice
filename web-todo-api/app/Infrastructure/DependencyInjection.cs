using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database configuration
            var connectionString = GetConnectionString(configuration);
            services.AddDbContext<AppDb>(options =>
                options.UseNpgsql(connectionString));

            // Repositories
            services.AddScoped<ITodoRepository, TodoRepository>();

            return services;
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            string? configured = configuration.GetConnectionString("Db");

            string host = Environment.GetEnvironmentVariable("POSTGRES_CONTAINER_NAME") ?? "db";
            string port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
            string? user = Environment.GetEnvironmentVariable("POSTGRES_USER");
            string? pwd = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
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
            return !string.IsNullOrWhiteSpace(configured) ? configured : fromEnv;
        }
    }
}

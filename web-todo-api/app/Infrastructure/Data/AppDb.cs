using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDb : DbContext
    {
        public AppDb(DbContextOptions<AppDb> options) : base(options) { }
        public DbSet<Todo> Todos => Set<Todo>();

        protected override void OnModelCreating(ModelBuilder m)
        {
            m.Entity<Todo>().HasKey(t => t.Id);
            m.Entity<Todo>().Property(t => t.Title).IsRequired().HasMaxLength(200);
            m.Entity<Todo>().HasIndex(t => new { t.Done, t.Title });
        }
    }
}
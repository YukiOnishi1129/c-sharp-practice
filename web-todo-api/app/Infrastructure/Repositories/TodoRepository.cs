using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly AppDb _context;

        public TodoRepository(AppDb context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Todo>> GetAllAsync(
            bool? done = null,
            string? titleContains = null,
            int? skip = null,
            int? take = null,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Todos.AsNoTracking().AsQueryable();

            if (done.HasValue)
                query = query.Where(t => t.Done == done.Value);

            if (!string.IsNullOrWhiteSpace(titleContains))
                query = query.Where(t => EF.Functions.ILike(t.Title, $"%{titleContains}%"));

            query = query.OrderBy(t => t.Title);

            if (skip.HasValue && skip.Value > 0)
                query = query.Skip(skip.Value);

            if (take.HasValue && take.Value > 0)
                query = query.Take(Math.Min(take.Value, 100)); // 上限100

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Todo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Todos
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<Todo> CreateAsync(Todo todo, CancellationToken cancellationToken = default)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync(cancellationToken);
            return todo;
        }

        public async Task<Todo> UpdateAsync(Todo todo, CancellationToken cancellationToken = default)
        {
            _context.Todos.Update(todo);
            await _context.SaveChangesAsync(cancellationToken);
            return todo;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var todo = await _context.Todos.FindAsync(new object[] { id }, cancellationToken);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Todos
                .AnyAsync(t => t.Id == id, cancellationToken);
        }
    }
}
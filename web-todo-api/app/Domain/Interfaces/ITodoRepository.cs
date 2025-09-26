using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITodoRepository
    {
        Task<IEnumerable<Todo>> GetAllAsync(
            bool? done = null,
            string? titleContains = null,
            int? skip = null,
            int? take = null,
            CancellationToken cancellationToken = default);
        
        Task<Todo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Todo> CreateAsync(Todo todo, CancellationToken cancellationToken = default);
        Task<Todo> UpdateAsync(Todo todo, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
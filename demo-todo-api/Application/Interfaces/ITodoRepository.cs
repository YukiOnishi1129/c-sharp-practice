using Domain.Entities;

namespace Application.Interfaces;

public interface ITodoRepository
{
	Task<List<Todo>> ListAsync(CancellationToken ct = default);
	Task<Todo?> GetByIdAsync(Guid id, CancellationToken ct = default);
	Task AddAsync(Todo todo, CancellationToken ct = default);
	Task<bool> UpsertAsync(Todo todo, CancellationToken ct = default);
	Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
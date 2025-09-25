using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class TodoService
{
	private readonly ITodoRepository _repo;
	public TodoService(ITodoRepository repo) => _repo = repo;

	public Task<List<Todo>> ListAsync(CancellationToken ct = default) => _repo.ListAsync(ct);
	public Task<Todo?> GetAsync(Guid id, CancellationToken ct = default) => _repo.GetByIdAsync(id, ct);
	public async Task<Todo> CreateAsync(string title, CancellationToken ct = default)
	{
		if (string.IsNullOrWhiteSpace(title))
			throw new ArgumentException("Title is required", nameof(title));

		var todo = new Todo(Guid.NewGuid(), title, false);
		await _repo.AddAsync(todo, ct);
		return todo;
	}
	public async Task<Todo?> UpdateAsync(Guid id, string title, bool done, CancellationToken ct = default)
	{
		var current = await _repo.GetByIdAsync(id, ct);
		if (current is null) return null;

		var updated = current with { Title = title, Done = done };
		await _repo.UpsertAsync(updated, ct);
		return updated;
	}
	public async Task<Todo?> ToggleAsync(Guid id, CancellationToken ct = default)
	{
		var current = await _repo.GetByIdAsync(id, ct);
		if (current is null) return null;

		var updated = current with { Done = !current.Done };
		await _repo.UpsertAsync(updated, ct);
		return updated;
	}

	public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default) => _repo.DeleteAsync(id, ct);
}
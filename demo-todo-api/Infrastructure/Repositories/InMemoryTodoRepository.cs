using System.Collections.Concurrent;
using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class InMemoryTodoRepository : ITodoRepository
{
	private readonly ConcurrentDictionary<Guid, Todo> _store = new();

	public Task AddAsync(Todo todo, CancellationToken ct = default)
	{
		_store[todo.Id] = todo;
		return Task.CompletedTask;
	}

	public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
		=> Task.FromResult(_store.TryRemove(id, out _));

	public Task<Todo?> GetByIdAsync(Guid id, CancellationToken ct = default)
	{
		_store.TryGetValue(id, out var todo);
		return Task.FromResult(todo);
	}

	public Task<List<Todo>> ListAsync(CancellationToken ct = default)
		=> Task.FromResult(_store.Values.OrderBy(t => t.Title).ToList());

	public Task<bool> UpsertAsync(Todo todo, CancellationToken ct = default)
	{
		_store[todo.Id] = todo;
		return Task.FromResult(true);
	}
}
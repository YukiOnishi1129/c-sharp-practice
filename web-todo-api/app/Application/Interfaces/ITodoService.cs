using Application.DTOs;

namespace Application.Interfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoDto>> GetAllAsync(TodoFilterDto? filter, CancellationToken cancellationToken = default);

        Task<TodoDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<TodoDto> CreateAsync(CreateTodoDto createDto, CancellationToken cancellationToken = default);

        Task<TodoDto?> UpdateAsync(Guid id, UpdateTodoDto updateDto, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

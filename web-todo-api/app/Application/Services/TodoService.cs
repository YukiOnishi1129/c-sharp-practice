using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        public async Task<IEnumerable<TodoDto>> GetAllAsync(TodoFilterDto? filter, CancellationToken cancellationToken = default)
        {
            var todos = await todoRepository.GetAllAsync(
                filter?.Done,
                filter?.TitleContains,
                filter?.Skip,
                filter?.Take,
                cancellationToken);

            return todos.Select(MapToDto);
        }

        public async Task<TodoDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var todo = await todoRepository.GetByIdAsync(id, cancellationToken);
            return todo != null ? MapToDto(todo) : null;
        }

        public async Task<TodoDto> CreateAsync(CreateTodoDto createDto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            var todo = new Todo
            {
                Title = createDto.Title,
                Done = createDto.Done,
            };

            var createdTodo = await todoRepository.CreateAsync(todo, cancellationToken);
            return MapToDto(createdTodo);
        }

        public async Task<TodoDto?> UpdateAsync(Guid id, UpdateTodoDto updateDto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(updateDto);

            var todo = await todoRepository.GetByIdAsync(id, cancellationToken);
            if (todo == null)
            {
                return null;
            }

            if (updateDto.Title != null)
            {
                todo.Title = updateDto.Title;
            }

            if (updateDto.Done.HasValue)
            {
                todo.Done = updateDto.Done.Value;
            }

            var updatedTodo = await todoRepository.UpdateAsync(todo, cancellationToken);
            return MapToDto(updatedTodo);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (!await todoRepository.ExistsAsync(id, cancellationToken))
            {
                return false;
            }

            await todoRepository.DeleteAsync(id, cancellationToken);
            return true;
        }

        private static TodoDto MapToDto(Todo todo)
        {
            return new TodoDto
            {
                Id = todo.Id,
                Title = todo.Title,
                Done = todo.Done,
            };
        }
    }
}

using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Presentation.Endpoints
{
    public static class TodoEndpoints
    {
        public static void MapTodoEndpoints(this IEndpointRouteBuilder app)
        {
            var todos = app.MapGroup("/todos")
                .WithTags("Todos")
                .WithOpenApi();

            todos.MapGet("/", GetAllTodos)
                .WithName("GetAllTodos")
                .WithSummary("Get all todos with optional filtering");

            todos.MapGet("/{id:guid}", GetTodoById)
                .WithName("GetTodoById")
                .WithSummary("Get a specific todo by ID");

            todos.MapPost("/", CreateTodo)
                .WithName("CreateTodo")
                .WithSummary("Create a new todo");

            todos.MapPut("/{id:guid}", UpdateTodo)
                .WithName("UpdateTodo")
                .WithSummary("Update an existing todo");

            todos.MapDelete("/{id:guid}", DeleteTodo)
                .WithName("DeleteTodo")
                .WithSummary("Delete a todo");
        }

        private static async Task<Results<Ok<IEnumerable<TodoDto>>, BadRequest>> GetAllTodos(
            ITodoService todoService,
            bool? done,
            string? q,
            int? skip,
            int? take,
            CancellationToken cancellationToken)
        {
            if (skip < 0 || take < 0)
            {
                return TypedResults.BadRequest();
            }

            var filter = new TodoFilterDto
            {
                Done = done,
                TitleContains = q,
                Skip = skip,
                Take = take,
            };

            var todos = await todoService.GetAllAsync(filter, cancellationToken);
            return TypedResults.Ok(todos);
        }

        private static async Task<Results<Ok<TodoDto>, NotFound>> GetTodoById(
            ITodoService todoService,
            Guid id,
            CancellationToken cancellationToken)
        {
            var todo = await todoService.GetByIdAsync(id, cancellationToken);
            return todo != null
                ? TypedResults.Ok(todo)
                : TypedResults.NotFound();
        }

        private static async Task<Results<Created<TodoDto>, BadRequest<string>>> CreateTodo(
            ITodoService todoService,
            CreateTodoDto createDto,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(createDto.Title))
            {
                return TypedResults.BadRequest("Title is required");
            }

            if (createDto.Title.Length > 200)
            {
                return TypedResults.BadRequest("Title must be 200 characters or less");
            }

            var todo = await todoService.CreateAsync(createDto, cancellationToken);
            return TypedResults.Created($"/todos/{todo.Id}", todo);
        }

        private static async Task<Results<Ok<TodoDto>, NotFound, BadRequest<string>>> UpdateTodo(
            ITodoService todoService,
            Guid id,
            UpdateTodoDto updateDto,
            CancellationToken cancellationToken)
        {
            if (updateDto.Title != null && string.IsNullOrWhiteSpace(updateDto.Title))
            {
                return TypedResults.BadRequest("Title cannot be empty");
            }

            if (updateDto.Title?.Length > 200)
            {
                return TypedResults.BadRequest("Title must be 200 characters or less");
            }

            var todo = await todoService.UpdateAsync(id, updateDto, cancellationToken);
            return todo != null
                ? TypedResults.Ok(todo)
                : TypedResults.NotFound();
        }

        private static async Task<Results<NoContent, NotFound>> DeleteTodo(
            ITodoService todoService,
            Guid id,
            CancellationToken cancellationToken)
        {
            var deleted = await todoService.DeleteAsync(id, cancellationToken);
            return deleted
                ? TypedResults.NoContent()
                : TypedResults.NotFound();
        }
    }
}

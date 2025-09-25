using Application.Services;
using Web.Contracts;

namespace Web.Endpoints;

public static class TodoEndpoints
{
	public static RouteGroupBuilder MapTodoEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/todos");

		group.MapGet("/", async (TodoService svc, CancellationToken ct) =>
		{
			var list = await svc.ListAsync(ct);
			return Results.Ok(list.ConvertAll(TodoResponse.From));
		});

		group.MapGet("/{id:guid}", async (TodoService svc, Guid id, CancellationToken ct) =>
		{
			var todo = await svc.GetAsync(id, ct);
			return todo is not null ? Results.Ok(TodoResponse.From(todo)) : Results.NotFound();
		});

		group.MapPost("/", async (TodoService svc, CreateTodoRequest req, CancellationToken ct) =>
		{
			if (string.IsNullOrWhiteSpace(req.Title))
				return Results.BadRequest(new { Error = "Title is required" });
			var created = await svc.CreateAsync(req.Title, ct);
			return Results.Created($"/todos/{created.Id}", TodoResponse.From(created));
		});

		group.MapPut("/{id:guid}", async (TodoService svc, Guid id, UpdateTodoRequest req, CancellationToken ct) =>
		{
			var updated = await svc.UpdateAsync(id, req.Title, req.Done, ct);
			return updated is not null ? Results.Ok(TodoResponse.From(updated)) : Results.NotFound();
		});

		group.MapPatch("/{id:guid}/toggle", async (TodoService svc, Guid id, CancellationToken ct) =>
		{
			var toggled = await svc.ToggleAsync(id, ct);
			return toggled is not null ? Results.Ok(TodoResponse.From(toggled)) : Results.NotFound();
		});

		group.MapDelete("/{id:guid}", async (TodoService svc, Guid id, CancellationToken ct) =>
		{
			var deleted = await svc.DeleteAsync(id, ct);
			return deleted ? Results.NoContent() : Results.NotFound();
		});

		return group;
	}

}
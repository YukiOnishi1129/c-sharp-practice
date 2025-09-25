using Domain.Entities;

namespace Web.Contracts;

public record TodoResponse(Guid Id, string Title, bool Done)
{
	public static TodoResponse From(Todo t) => new(t.Id, t.Title, t.Done);
}
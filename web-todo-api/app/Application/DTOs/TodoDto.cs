namespace Application.DTOs
{
    public class TodoDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public bool Done { get; set; }
    }

    public class CreateTodoDto
    {
        public string Title { get; set; } = string.Empty;

        public bool Done { get; set; } = false;
    }

    public class UpdateTodoDto
    {
        public string? Title { get; set; }

        public bool? Done { get; set; }
    }

    public class TodoFilterDto
    {
        public bool? Done { get; set; }

        public string? TitleContains { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }
    }
}

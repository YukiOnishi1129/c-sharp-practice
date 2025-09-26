namespace Domain.Entities
{
    public class Todo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public bool Done { get; set; } = false;
    }
}
namespace TodoWeb.Application.DTOs
{
    public class ToDoUpdateModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCompeleted { get; set; }
    }
}

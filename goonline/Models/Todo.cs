namespace goonline.Models
{
    public class Todo
    {
        public int id { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public DateTime expiryDate { get; set; }
        public int percentComplete { get; set; }
        public bool isDone { get; set; }
    }
}

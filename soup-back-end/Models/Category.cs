namespace soup_back_end.Models
{
    public class Category
    {
        public string category_id { get; set; } = string.Empty;
        public string category_name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string img { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}

using System;
using soup_back_end.Data;

namespace soup_back_end.Models
{
    public class Course
    {
        public string Id { get; set; } = string.Empty;
        public string categoryId { get; set; } = string.Empty;
        public string course_Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string img { get; set; } = string.Empty;
        public int course_price { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}

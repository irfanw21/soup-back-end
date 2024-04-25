using System;

namespace soup_back_end.DTOs.Category
{
    public class CategoryDTO
    {
        public string category_id { get; set; } = string.Empty;
        public string category_name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string img { get; set; } = string.Empty;
    }
}

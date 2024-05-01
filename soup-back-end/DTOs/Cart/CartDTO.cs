using Microsoft.OpenApi.Writers;

namespace soup_back_end.DTOs.Cart
{
    public class CartDTO
    {
        public string courseId { get; set; } = string.Empty;
        public string categoryId { get; set; } = string.Empty;
        public Guid userId { get; set; }
        public string scheduleId {  get; set; } = string.Empty;
    }
}
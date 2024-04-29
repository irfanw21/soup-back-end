namespace soup_back_end.Models
{
    public class Cart
    {
        public Guid cartId { get; set; }
        public string courseId { get; set; } = string.Empty;
        public string categoryId { get; set; } = string.Empty;
    }
}

namespace soup_back_end.DTOs.Invoice
{
    public class InvoiceDTO
    {
        public Guid cartId { get; set; }
        public string courseId { get; set; } = string.Empty;
        public string categoryId { get; set; } = string.Empty;
        public string paymentId { get; set; } = string.Empty;
    }
}

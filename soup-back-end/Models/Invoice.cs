namespace soup_back_end.Models
{
    public class Invoice
    {
        public Guid invoiceId {  get; set; }
        public Guid cartId { get; set; }
        public string courseId { get; set; } = string.Empty;
        public string categoryId { get; set; } = string.Empty;
        public string paymentId { get; set; } = string.Empty;
    }
}

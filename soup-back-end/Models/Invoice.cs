namespace soup_back_end.Models
{
    public class Invoice
    {
        public Guid invoiceId {  get; set; }
        public string paymentId { get; set; } = string.Empty;
        public Guid userId { get; set; } = Guid.Empty;
        public DateTime invoiceDate { get; set; }
        public decimal totalPaid { get; set; }
        public int itemCount { get; set; }
    }
}

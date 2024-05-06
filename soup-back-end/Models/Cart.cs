﻿namespace soup_back_end.Models
{
    public class Cart
    {
        public Guid cartId { get; set; }
        public string courseId { get; set; } = string.Empty;
        public string categoryId { get; set; } = string.Empty;
        public Guid userId { get; set; } = Guid.Empty;
        public string scheduleId {  get; set; } = string.Empty;
        public bool isSelected { get; set; }
        public  Guid invoiceId { get; set; } = Guid.Empty;
    }
}

namespace PandaWebApp.Models
{
    using System;

    public class Receipt
    {
        public string Id { get; set; }

        public decimal Fee { get; set; }

        public DateTime IssuedOn { get; set; } = DateTime.UtcNow;

        public string RecipientId { get; set; }
        public virtual User Recipient { get; set; }

        public string PackageId { get; set; }
        public virtual Package Package { get; set; }
    }
}

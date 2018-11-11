namespace PandaWebApp.Models
{
    using System;

    public class Receipt
    {
        public int Id { get; set; }

        public decimal Fee { get; set; }

        public DateTime IssuedOn { get; set; } = DateTime.UtcNow;

        public virtual User Recipient { get; set; }

        public virtual Package Package { get; set; }
    }
}

namespace Psychiatrist_Management_System.Models
{
    public class Payment
    {
        public int BookingId { get; set; } // integer
        public string? PaymentMethod { get; set; }
        public string? AccountNumber { get; set; } // string instead of int
        public string? Pin { get; set; } // Add this
        public string? CardNumber { get; set; }
        public string? CardExpiryDate { get; set; }
        public string? CardCvv { get; set; }
        public int PaymentId { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}

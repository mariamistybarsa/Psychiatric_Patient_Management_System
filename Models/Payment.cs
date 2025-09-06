
namespace Psychiatrist_Management_System.Models
{
    public class Payment
    {
        public int Paymentid { get; set; }

        public int AccountNumber { get; set; }

        public string? PaymentMethod { get; set; }
        public string? CardExpiryDate { get; set; }
        public string? CardCvv { get; set; }

        public int BookingId { get; set; }











    }
}

namespace Psychiatrist_Management_System.Models
{
    public class PaymentVM
    {

      
            public int PaymentId { get; set; }
            public string UserName { get; set; }
            public int BookingId { get; set; }
            public DateTime AppointmentDate { get; set; }
            public decimal RefundAmount { get; set; }
            public decimal PaymentAmount { get; set; }
            public string PaymentMethod { get; set; }
            public decimal TestFee { get; set; }
            public decimal VisitFee { get; set; }

        public DateTime CreatedDate { get; set; }
         public string BookingSerial { get; set; }
        public string  PsychiatristName { get; set; }
        public string PaymentStatus { get; set; }




    }
}

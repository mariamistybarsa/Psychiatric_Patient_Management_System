namespace Psychiatrist_Management_System.Models
{
    public class BookingHistoryVM
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }

        public int PsychiatristId { get; set; }
        public string UserName { get; set; }
        public string PatientName { get; set; }       // new
        public string PsychiatristName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string AppointmentDay { get; set; }
        public string Notes { get; set; }

        // Add these
        public string ApprovalStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string Email { get; set; }
        public string UserEmail { get; set; }
        
        public int Age { get; set; }
        public string? Address { get; set; }
        public string? UserAddress { get; set; }
      
        public string? BloodGroup { get; set; }
        public string? Status { get; set; }
        public string? DesignationName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserPhoneNumber { get; set; }
        
        public string? Specialization { get; set; }
        public string? Experience { get; set; }
        public string? EmergencyContact { get; set; }
        public decimal? VisitFee { get; set; }
        public decimal? TestFee { get; set; }
        public int? PaymentAmount { get; set; }
        public int? HoursDifference { get; set; }
        public decimal RefundAmount { get; set; }
        public decimal BookingSerial { get; set; }
        // Booking Info
      
        public DateTime CreatedAt { get; set; }
       
        public string? PaymentMethod { get; set; }
        public string? AccountNumber { get; set; }
       

    }
}

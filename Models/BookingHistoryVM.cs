namespace Psychiatrist_Management_System.Models
{
    public class BookingHistoryVM
    {
        public int BookingId { get; set; }
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
    }
}

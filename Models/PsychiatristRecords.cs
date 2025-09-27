namespace Psychiatrist_Management_System.Models
{
    public class PsychiatristRecords
    {
        public int PsychiatristId { get; set; } 
        public string PsychiatristName { get; set; }
        public string ApprovalStatus { get; set; }
       
        public int BookingId { get; set; }
         // optional if using ViewBag
        public string PatientName { get; set; }       // useful for clarity
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
  
        public DateTime? ApprovedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string CancelReason { get; set; }
        public DateTime? CompletedAt { get; set; }

        public string Notes { get; set; }

        public int Records { get; set; }


    }
}

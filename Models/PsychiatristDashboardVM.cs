namespace Psychiatrist_Management_System.Models
{
    public class PsychiatristDashboardVM
    {
        // Summary
        public int UserId { get; set; }
        public string PatientName { get; set; }
        public string ApprovalStatus { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Review info
        public string Comment { get; set; }
        public int? Rate { get; set; }

        // Summary counts
        public int ApprovedPatients { get; set; }
        public int PaidPatients { get; set; }
        public int CompletedPatients { get; set; }
    }
}

namespace Psychiatrist_Management_System.Models
{
    public class GetMonthlySummary
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int PsychiatristId { get; set; }
        public string PsychiatristName { get; set; } // Name যোগ করা
        public int Pending { get; set; }
        public int Running { get; set; }
        public int Completed { get; set; }
        public int TotalUsers { get; set; }
    }
}

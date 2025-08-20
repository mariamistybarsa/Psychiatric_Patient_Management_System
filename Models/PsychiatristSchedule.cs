namespace Psychiatrist_Management_System.Models
{
    public class PsychiatristSchedule
    {
        public int PsychiatristScheduleId { get; set; } = 0;
        public int? PsychiatristId { get; set; }
        public  String StartDay { get; set; }
        public  String EndDay{ get; set; }
        public String StartTime { get; set; }
     
        public string EndTime { get; set; }
        public string Status { get; set; }
    }
}

namespace Psychiatrist_Management_System.Models
{
    public class GetScheduleByAppointmentDate
    {
        public int? PsychiatristId { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }
}

namespace Psychiatrist_Management_System.Models
{
    public class BookVM
    {
public int BookingId { get; set; }
        public int PsychiatristId { get; set; }
        public int UserId { get; set; }
        public int PsychiatristScheduleId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string AppointmentDay { get; set; }
        public string notes { get; set; }

        public string Status { get; set; }  
    }
}

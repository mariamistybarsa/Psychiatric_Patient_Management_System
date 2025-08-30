namespace Psychiatrist_Management_System.Models
{
    public class BookingHistoryVM
    {
        public int BookingId { get; set; }
        public string PsychiatristName { get; set; } // <-- add this
        public string UserName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string AppointmentDay { get; set; }
        public string Notes { get; set; }
    }
}

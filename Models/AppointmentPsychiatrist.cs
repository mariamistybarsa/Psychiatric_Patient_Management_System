//AppointmentPsychiatrist

using System.ComponentModel.DataAnnotations;

namespace Psychiatrist_Management_System.Models
{
    public class AppointmentPsychiatrist
    {
        
        public int PsyAppId { get; set; }
        public int UserId { get; set; }
        public string Startday { get; set; }
        public string Endday { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public string? Status { get; set; }
    }
}

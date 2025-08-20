namespace Psychiatrist_Management_System.Models
{
    public class PsyProfileVM
    {
        public int ProfileId { get; set; } = 0;
        public int? PsychiatristId { get; set; }
        public string? Bio { get; set; }
      public string? Specialization { get; set; }
        public string? Experience { get; set; }
        public string? EmergencyContact { get; set; }
        public string? Imageurl { get; set; }


    }
}

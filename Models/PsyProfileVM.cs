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
        public string? UserName { get; set; }
        public string? Address { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public decimal VisitFee { get; set; }
        public decimal TestFee  { get; set; }



        public IFormFile? ImageFile { get; set; }


    }
}

namespace Psychiatrist_Management_System.Models
{
    public class PrescriptionVM

    {
        public int PrescriptionId { get; set; }

        public int BookingId { get; set; }
        public int Age { get; set; }
        public int UserId { get; set; }
        public int PatientId { get; set; }
        public int PsychiatristId { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public string Dose { get; set; }
        public string Prescribed_Notes { get; set; }
        public string Medicines { get; set; }
    }
}

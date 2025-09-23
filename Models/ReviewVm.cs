namespace Psychiatrist_Management_System.Models
{
    public class ReviewVm
    {
        public int ReviewId { get; set; }
        public int BookingId { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public string PatientName { get; set; }
    }
}

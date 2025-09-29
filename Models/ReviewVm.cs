namespace Psychiatrist_Management_System.Models
{
    public class ReviewVm
    {
       
            public int ReviewId { get; set; }
            public int BookingId { get; set; }
            public int Rate { get; set; }
            public string Comment { get; set; }
            public string PatientName { get; set; }
            public string BookingSerial { get; set; }
            public DateTime CreatedAt { get; set; }
            public string AppointmentDate { get; set; }
   


    }
}

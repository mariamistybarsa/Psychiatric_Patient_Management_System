//PrescriptionVM

namespace Psychiatrist_Management_System.Models
{
    public class PrescriptionVM

    {
        
        public int MedicinePrescriptionId { get; set; }
        public int PrescriptionId { get; set; }
        
        public int MedicineId { get; set; }
        public int BookingId { get; set; }
        public int Age { get; set; }
        public int UserId { get; set; }
        
        public string MedicineDuration { get; set; }
        public string Medicine_Notes { get; set; }
     
        public string MedicineDose { get; set; }
       
        public string Frequency { get; set; }
        public string Advice { get; set; }
  
    }
}

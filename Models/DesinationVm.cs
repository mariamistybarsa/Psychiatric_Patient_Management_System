using System.ComponentModel.DataAnnotations;

namespace Psychiatrist_Management_System.Models
{
    public class DesinationVm
    {
        [Key]
        public int DesignationId { get; set; }
        public string? DesignationName { get; set; }
  
    }
}

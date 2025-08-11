using System.ComponentModel.DataAnnotations;

namespace Psychiatrist_Management_System.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public int UsertypeId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

    }
}

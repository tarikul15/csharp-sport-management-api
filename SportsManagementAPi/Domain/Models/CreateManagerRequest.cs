using System.ComponentModel.DataAnnotations;

namespace SportsManagementAPi.Domain.Models
{
    public class CreateManagerRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(32)]
        public string Password { get; set; }
    }
}
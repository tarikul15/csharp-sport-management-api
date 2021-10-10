using System.ComponentModel.DataAnnotations;

namespace SportsManagementAPi.Domain.Models
{
    public class RevokeTokenResource
    {
        [Required]
        public string Token { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.DTOs.Auth
{
    public class BaseDto
    {
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

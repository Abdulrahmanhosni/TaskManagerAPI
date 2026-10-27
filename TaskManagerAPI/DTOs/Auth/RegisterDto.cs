using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace TaskManagerAPI.DTOs.Auth
{
    public class RegisterDto : BaseDto
    {
        [StringLength(30)]
        public string Name { get; set; }
    }
}

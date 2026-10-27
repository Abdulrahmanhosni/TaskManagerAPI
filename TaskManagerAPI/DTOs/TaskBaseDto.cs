using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.DTOs
{
    public class TaskBaseDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        public string Priority { get; set; }
    }
}

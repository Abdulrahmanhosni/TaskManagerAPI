namespace TaskManagerAPI.DTOs
{
    public class TaskResponseDto : TaskBaseDto
    {
        public int Id { get; set; }
        public bool IsCompleted { get; set; }
    }
}

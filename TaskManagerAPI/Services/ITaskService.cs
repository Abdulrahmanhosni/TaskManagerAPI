using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
        public interface ITaskService
        {
            Task<PagedResult<TaskResponseDto>> GetAllTasksAsync
                (
                string? title,
                string? priority,
                int pageNumber,
                int pageSize,
                int userId);
            Task<TaskResponseDto> GetTaskByIdAsync(int id, int userId);
            Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto taskData,int userId);
            Task <TaskResponseDto> UpdateTaskAsync(int  id, UpdateTaskDto taskData, int userId);
            Task DeleteTaskAsync(int id, int userId);
        }
}

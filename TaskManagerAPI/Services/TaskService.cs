using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Exceptions;
using TaskManagerAPI.Models;
namespace TaskManagerAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public TaskService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<PagedResult<TaskResponseDto>> GetAllTasksAsync(string? title, string? priority, int pageNumber, int pageSize, int userId)
        {
            var query = _context.Tasks.Where(t => t.UserId == userId).AsQueryable();

            if (title != null)
                query = query.Where(x => x.Title == title);
            if (priority != null)
                query = query.Where(p => p.Priority == priority);


            int totalCount = await query.CountAsync();
            double total = (double)totalCount / pageSize;
            int totalPages = (int)Math.Ceiling(total);
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var data = await query.ToListAsync();
            var result = _mapper.Map<IEnumerable<TaskResponseDto>>(data);

            return new PagedResult<TaskResponseDto>
            {
                Data = result,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }
        public async Task<TaskResponseDto> GetTaskByIdAsync(int id, int userId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (task == null) throw new NotFoundException("Task not found");
            return _mapper.Map<TaskResponseDto>(task);
        }
        
        public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto taskData, int userId)
        {

            var task = _mapper.Map<TaskItem>(taskData);
            task.UserId = userId;
            _context.Tasks.Add(task); 
            await _context.SaveChangesAsync();
            return _mapper.Map<TaskResponseDto>(task);
        }
        
        public async Task<TaskResponseDto> UpdateTaskAsync(int id, UpdateTaskDto taskData,int userId)
        {
           
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (task == null) throw new NotFoundException("Task not found");
            task.Title = taskData.Title;
            task.Priority = taskData.Priority;
            task.Description = taskData.Description;
            task.IsCompleted = taskData.IsCompleted;
            
            await _context.SaveChangesAsync();
            return _mapper.Map<TaskResponseDto>(task);
        }
        
        public async Task DeleteTaskAsync(int id, int userId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
                throw new NotFoundException("Task not found");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}


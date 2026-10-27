using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;
using System.Security.Claims;
namespace TaskManagerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        //private readonly IMapper _mapper;
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
           // _mapper = mapper;
        }
        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        }
        /// <summary>
        /// Returns all user tasks with filtering and pagination capabilities.
        /// </summary>
        /// <param name="title">Filter by title</param>
        /// <param name="priority">Filter by priority</param>
        /// <param name="pageNumber">Page number, default is 1</param>
        /// <param name="pageSize">Number of items per page, default is 10</param>
        /// <returns>List of tasks with pagination metadata</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllTasks
            (string? title, string? priority, int pageNumber= 1, int pageSize = 10)
        {
            var userId = GetUserId();
            var tasks = await _taskService.GetAllTasksAsync(title, priority,pageNumber,pageSize,userId);
            
            return Ok(tasks);
        }
        /// <summary>
        /// Returns a specific task by its ID for the authenticated user.
        /// </summary>
        /// <param name="id">The task ID</param>
        /// <returns>The task details</returns>
        /// <remarks>
        /// Only returns tasks that belong to the authenticated user.
        /// Example request: GET /api/tasks/5
        /// </remarks>
        /// <exception cref="NotFoundException">Thrown when task is not found or doesn't belong to the user</exception>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var userId = GetUserId();
            var task = await _taskService.GetTaskByIdAsync(id,userId);
            return Ok(task);
        }
        /// <summary>
        ///  Creates a new task for the authenticated user.
        /// </summary>
        /// <param name="taskData">The task data to create</param>
        /// <returns>The created task details</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateTask([FromBody]CreateTaskDto taskData)
        {
            var userId = GetUserId();
            var newTask = await _taskService.CreateTaskAsync(taskData, userId);
            return CreatedAtAction(nameof(GetTaskById), new { id = newTask.Id }, newTask);
        }
        /// <summary>
        /// Updates a task for the authenticated user.
        /// </summary>
        /// <param name="id">The task ID</param>
        /// <param name="taskData">The updated task data</param>
        /// <returns>The updated task details</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto taskData)
        {
            var userId = GetUserId();
            var updatetask = await _taskService.UpdateTaskAsync(id, taskData,userId);
            
            return Ok(updatetask);
        }
        /// <summary>
        /// Deletes a specific task by its ID for the authenticated user.
        /// </summary>
        /// <param name="id">The task ID to delete</param>
        /// <returns>>No content on success</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTask(int id)
        { 
            var userId = GetUserId();
            await _taskService.DeleteTaskAsync(id,userId);
            return NoContent();
        }
    }
}
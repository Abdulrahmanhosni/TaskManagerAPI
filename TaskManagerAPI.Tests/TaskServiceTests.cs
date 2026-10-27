using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Exceptions;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;

public class TaskServiceTests
{
    private readonly TaskService _service;
    private readonly AppDbContext _context;

    public TaskServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);

        var mockMapper = new Mock<IMapper>();
        mockMapper
            .Setup(m => m.Map<TaskResponseDto>(It.IsAny<TaskItem>()))
            .Returns(new TaskResponseDto { Id = 1, Title = "Test Task" });

        _service = new TaskService(_context, mockMapper.Object);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ExistingId_ReturnsTask()
    {
        _context.Tasks.Add(new TaskItem
        {
            Id = 1,
            Title = "Test Task",
            Description = "Test Description",
            Priority = "High",
            UserId = 1
        });
        await _context.SaveChangesAsync();

        var result = await _service.GetTaskByIdAsync(1, 1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Task", result.Title);
    }

    [Fact]
    public async Task GetTaskByIdAsync_NonExistingId_ThrowsNotFoundException()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetTaskByIdAsync(99, 1));
    }
    [Fact]
    public async Task GetTaskByIdAsync_TaskOfAnotherUser_ThrowsNotFoundException()
    {
        _context.Tasks.Add(new TaskItem
        {
            Id = 1,
            Title = "Test Task",
            Description = "Test Description",
            Priority = "High",
            UserId = 1
        });
        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<NotFoundException>(
            () => _service.GetTaskByIdAsync(1, 99));
    }
}
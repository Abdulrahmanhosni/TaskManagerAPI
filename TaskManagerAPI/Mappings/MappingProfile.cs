using AutoMapper;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<TaskItem, TaskResponseDto>();
            CreateMap<CreateTaskDto, TaskItem>();
            CreateMap<UpdateTaskDto, TaskItem>();
        }
    }
}

using AutoMapper;
using Tasker.Application.DTOs;
using Tasker.Domain.Entities.Application;
using TaskStatus = Tasker.Domain.Entities.Application.TaskStatus;

namespace Tasker.Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<KanbanBoard, KanbanBoardDto>().ReverseMap();
        CreateMap<Release, ReleaseDto>().ReverseMap();
        CreateMap<TaskStatus, TaskStatusDto>().ReverseMap();
    }
}
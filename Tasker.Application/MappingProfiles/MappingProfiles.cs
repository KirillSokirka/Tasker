using AutoMapper;
using Tasker.Application.DTOs;
using Tasker.Application.DTOs.Application;
using Tasker.Application.DTOs.Application.KanbanBoard;
using Tasker.Application.DTOs.Application.Task;
using Tasker.Domain.Entities.Application;
using Task = Tasker.Domain.Entities.Application.Task;
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
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Task, TaskDto>();
        CreateMap<TaskDto, Task>()
            .ForMember(t => t.ProjectId, dto => dto.MapFrom(t => t.Project.Id))
            .ForMember(t => t.CreatorId, dto => dto.MapFrom(t => t.Creator.Id))
            .ForMember(t => t.TaskStatusId, dto => dto.MapFrom(t => t.Status != null ? t.Status.Id : null))
            .ForMember(t => t.ReleaseId, dto => dto.MapFrom(t => t.Release != null ? t.Release.Id : null))
            .ForMember(t => t.AssigneeId, dto => dto.MapFrom(t => t.Assignee != null ? t.Assignee.Id : null));
    }
}
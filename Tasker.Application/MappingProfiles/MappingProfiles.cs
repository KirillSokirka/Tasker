using AutoMapper;
using AutoMapper.Execution;
using Tasker.Application.DTOs;
using Tasker.Application.DTOs.Application;
using Tasker.Application.DTOs.Application.Task;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Enums;
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

        CreateMappingForUpdateDto();
    }

    private void CreateMappingForUpdateDto()
    {
        CreateMap<TaskUpdateDto, Task>()
            .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title != null))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
            .ForMember(dest => dest.Priority, opt => opt.Condition(src => src.Priority != TaskPriority.None))
            .ForMember(dest => dest.ProjectId,
                opt => opt.Condition(src => src.Project is { Id: not null }))
            .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.Project!.Id))
            .ForMember(dest => dest.CreatorId,
                opt => opt.Condition(src => src.Creator is { Id: not null }))
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Creator!.Id))
            .ForMember(dest => dest.TaskStatusId,
                opt => opt.Condition(src => src.Status is { Id: not null }))
            .ForMember(dest => dest.TaskStatusId,
                opt => opt.MapFrom(src => src.Status != null ? src.Status.Id : null))
            .ForMember(dest => dest.ReleaseId,
                opt => opt.Condition(src => src.Release is { Id: not null }))
            .ForMember(dest => dest.ReleaseId,
                opt => opt.MapFrom(src => src.Release != null ? src.Release.Id : null))
            .ForMember(dest => dest.AssigneeId,
                opt => opt.Condition(src => src.Assignee is { Id: not null }))
            .ForMember(dest => dest.AssigneeId,
                opt => opt.MapFrom(src => src.Assignee != null ? src.Assignee.Id : null));
    }
}
using AutoMapper;
using Tasker.Application.DTOs.Application;
using Tasker.Application.DTOs.Application.KanbanBoard;
using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.DTOs.Application.Release;
using Tasker.Application.DTOs.Application.Task;
using Tasker.Application.DTOs.Application.TaskStatus;
using Tasker.Domain.Entities.Application;
using Task = Tasker.Domain.Entities.Application.Task;
using TaskStatus = Tasker.Domain.Entities.Application.TaskStatus;

namespace Tasker.Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<Project, ProjectResultDto>()
            .ForMember(t => t.KanbanBoardIds, src => src.MapFrom(t => t.KanbanBoards.Select(val => val.Id)))
            .ForMember(t => t.TaskIds, src => src.MapFrom(t => t.Tasks.Select(val => val.Id)))
            .ForMember(t => t.ReleaseIds, src => src.MapFrom(t => t.Releases.Select(val => val.Id)));
        CreateMap<KanbanBoard, KanbanBoardDto>().ReverseMap();
        CreateMap<KanbanBoardUpdateDto, KanbanBoard>();
        CreateMap<KanbanBoard, KanbanBoardResultDto>()
            .ForMember(t => t.ColumnIds, src => src.MapFrom(t => t.Columns.Select(val => val.Id)));
        CreateMap<Release, ReleaseDto>().ReverseMap();
        CreateMap<Release, ReleaseCreateDto>().ReverseMap();
        CreateMap<TaskStatus, TaskStatusDto>().ReverseMap();
        CreateMap<TaskStatus, TaskStatusCreateDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Task, TaskDto>()
            .ForMember(t => t.ProjectId, dto => dto.MapFrom(t => t.Project.Id))
            .ForMember(t => t.Creator, dto => dto.MapFrom(t => t.Creator))
            .ForMember(t => t.TaskStatusId, dto => dto.MapFrom(t => t.Status != null ? t.Status.Id : null))
            .ForMember(t => t.ReleaseId, dto => dto.MapFrom(t => t.Release != null ? t.Release.Id : null))
            .ForMember(t => t.Assignee, dto => dto.MapFrom(t => t.Assignee != null ? t.Assignee : null));
    }
}
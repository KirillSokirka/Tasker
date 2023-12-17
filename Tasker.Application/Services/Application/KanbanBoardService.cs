using AutoMapper;
using Tasker.Application.DTOs.Application.KanbanBoard;
using Tasker.Application.Interfaces.Resolvers;
using Tasker.Application.Interfaces.Services;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Exceptions;
using Tasker.Domain.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Application.Services.Application;

public class KanbanBoardService : EntityService<KanbanBoard, KanbanBoardDto>, IKanbanBoardService
{
    private readonly IResolver<Project, string> _projectResolver;

    public KanbanBoardService(IEntityRepository<KanbanBoard> repository, IMapper mapper,
        IResolver<Project, string> projectResolver) : base(repository, mapper)
    {
        _projectResolver = projectResolver;
    }

    public async Task<KanbanBoardDto> CreateAsync(KanbanBoardCreateDto dto)
    {
        await Validate(dto.Title, dto.ProjectId);

        var entity = new KanbanBoard
        {
            Title = dto.Title,
            Project = await _projectResolver.ResolveAsync(dto.ProjectId),
            ProjectId = dto.ProjectId
        };
        
        await Repository.AddAsync(entity);

        return (await GetByIdAsync(entity.Id))!;
    }

    public async Task<KanbanBoardDto> UpdateAsync(KanbanBoardUpdateDto dto)
    {
        var entity = await Repository.GetByIdAsync(dto.Id) ??
                      throw new InvalidEntityException($"The kanban board with id {dto.Id} is not found");
        
        if (dto.Title is not null)
        {
            await Validate(dto.Title, entity.ProjectId);
            
            entity.Title = dto.Title ?? entity.Title;
        }
        
        await Repository.UpdateAsync(entity);
        
        return (await GetByIdAsync(entity.Id))!;
    }
    
    private async Task Validate(string title, string projectId)
    {
        var result =
            (await Repository.FindAsync(r => r.Title == title && r.ProjectId == projectId))
            .FirstOrDefault() is not null;

        if (!result)
        {
            throw new InvalidEntityException($"The same release is already exist in current project");
        }
    }
}
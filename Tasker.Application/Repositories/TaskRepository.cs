using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tasker.Application.DTOs;
using Tasker.Application.DTOs.Application;
using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.DTOs.Application.Task;
using Tasker.Application.EntitiesExtension;
using Tasker.Application.Interfaces.Repositories;
using Tasker.Application.Resolvers.DTOs;
using Tasker.Application.Resolvers.Interfaces;
using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;
using Task = Tasker.Domain.Entities.Application.Task;
using TaskStatus = Tasker.Domain.Entities.Application.TaskStatus;

namespace Tasker.Application.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly IResolver<TaskResolvedPropertiesDto, TaskUpdateDto> _taskResolver;
    private readonly IResolver<Project, string> _projectResolver;
    private readonly IResolver<User, string> _userResolver;
    private readonly IResolver<Release, string> _releaseResolver;
    private readonly IResolver<TaskStatus, string> _statusResolver;
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public TaskRepository(ApplicationContext context, IMapper mapper, 
        IResolver<TaskResolvedPropertiesDto, TaskUpdateDto> taskUpdateResolver,
        IResolver<User, string> userResolver,
        IResolver<Project, string> projectResolver,
        IResolver<Release, string> releaseResolver,
        IResolver<TaskStatus, string> statusResolver)
    {
        _projectResolver = projectResolver;
        _userResolver = userResolver;
        _taskResolver = taskUpdateResolver;
        _releaseResolver = releaseResolver;
        _statusResolver = statusResolver;
        _context = context;
        _mapper = mapper;
    }

    public async Task<TaskDto> CreateAsync(TaskCreateDto dto)
    {
        var task = new Task()
        {
            Title = dto.Title,
            Description = dto.Description,
            Creator = await _userResolver.ResolveAsync(dto.CreatorId),
            Project = await _projectResolver.ResolveAsync(dto.ProjectId),
            Release = dto.ReleaseId is null ? null : await _releaseResolver.ResolveAsync(dto.ReleaseId),
            Status = dto.TaskStatusId is null ? null : await _statusResolver.ResolveAsync(dto.TaskStatusId),
            Assignee = dto.AssigneeId is null ? null : await _userResolver.ResolveAsync(dto.AssigneeId)
        };

        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        return _mapper.Map<TaskDto>(task);
    }

    public async Task<TaskDto?> UpdateAsync(TaskUpdateDto dto)
    {
        var task =  await GetTaskEntity(dto.Id);

        if (task is null)
        {
            return null;
        }

        var resolvedProperties = await _taskResolver.ResolveAsync(dto);

        task.Update(dto, resolvedProperties);

        _context.Entry(task).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return _mapper.Map<TaskDto>(task);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task is null)
        {
            return false;
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<TaskDto?> GetAsync(string id)
    {
        var task = await GetTaskEntity(id);

        return task is not null ? _mapper.Map<TaskDto>(task) : null;
    }

    private async Task<Task?> GetTaskEntity(string id)
        => await _context.Tasks
            .Include(t => t.Project)
            .Include(t => t.Assignee)
            .Include(t => t.Release)
            .Include(t => t.Creator)
            .Include(t => t.Status)
            .AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    public async Task<List<TaskDto>> GetAllAsync() =>
        await _context.Tasks
            .Include(t => t.Project)
            .Include(t => t.Assignee)
            .Include(t => t.Release)
            .Include(t => t.Creator)
            .Include(t => t.Status)
            .AsNoTracking()
            .Select(task => _mapper.Map<TaskDto>(task))
            .ToListAsync();
}
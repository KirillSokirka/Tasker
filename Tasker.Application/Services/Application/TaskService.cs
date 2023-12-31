﻿using AutoMapper;
using Tasker.Application.DTOs.Application.Task;
using Tasker.Application.DTOs.Application.TaskStatus;
using Tasker.Application.EntitiesExtension;
using Tasker.Application.Interfaces.Resolvers;
using Tasker.Application.Interfaces.Services;
using Tasker.Domain.Repositories;
using Task = Tasker.Domain.Entities.Application.Task;

namespace Tasker.Application.Services.Application;

public class TaskService : EntityService<Task, TaskDto>, ITaskService
{
    private readonly ITaskResolver _taskResolver;

    public TaskService(IEntityRepository<Task> repository, ITaskResolver taskResolver,
        IMapper mapper) : base(repository, mapper)
    {
        _taskResolver = taskResolver;
    }

    public async Task<TaskDto> CreateAsync(TaskCreateDto createDto)
    {
        var task = await _taskResolver.ResolveAsync(createDto);

        task.CreationDate = DateTime.Now;
        task.Priority = createDto.Priority;
        
        await Repository.AddAsync(task);

        return (await GetByIdAsync(task.Id))!;
    }

    public async Task<TaskDto?> UpdateAsync(TaskUpdateDto dto)
    {
        var task = (await Repository.FindAsync(p => p.Id == dto.Id)).FirstOrDefault() 
                   ?? throw new Exception("The update failed. Try again");
        
        var resolvedProperties = await _taskResolver.ResolveAsync(dto);

        task.Update(dto, resolvedProperties);

        await Repository.UpdateAsync(task);

        return await GetByIdAsync(task.Id);
    }

    public async Task<TaskDto?> UpdateTaskStatusAsync(TaskUpdateStatusDto dto)
    {
        var task = (await Repository.FindAsync(p => p.Id == dto.Id)).FirstOrDefault() 
                   ?? throw new Exception("The update failed. Try again");
        
        var resolvedProperties = await _taskResolver.ResolveStatusAsync(dto);

        task.Status = resolvedProperties.Status;
        task.TaskStatusId = resolvedProperties.Status?.Id;
        
        await Repository.UpdateAsync(task);

        return await GetByIdAsync(task.Id);
    }
}
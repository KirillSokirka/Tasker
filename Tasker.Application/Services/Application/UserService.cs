using AutoMapper;
using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.DTOs.Application.User;
using Tasker.Application.EntitiesExtension;
using Tasker.Application.Interfaces;
using Tasker.Application.Interfaces.Resolvers;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Repositories;

namespace Tasker.Application.Services.Application;

public class UserService : EntityService<User, UserDto>, IUserService
{
    private readonly IUserResolver _resolver;

    public UserService(IEntityRepository<User> repository, IMapper mapper,
        IUserResolver resolver) : base(repository, mapper)
    {
        _resolver = resolver;
    }

    public async Task<UserDto> CreateAsync(UserDto createDto)
    {
        var user = Mapper.Map<User>(createDto);

        await Repository.AddAsync(user);

        return (await GetByIdAsync(user!.Id))!;
    }

    public async Task<UserDto?> UpdateAsync(UserUpdateDto dto)
    {
        var user = (await Repository.FindAsync(u => u.Title == dto.Username)).FirstOrDefault();

        if (user is null)
        {
            return null;
        }

        var (adminProjects, assignedProjects) = GetProjects(dto, user.Id);

        var resolvedProperties = await _resolver.ResolveAsync(
            admin: adminProjects,
            assigned: assignedProjects,
            userId: user.Id);

        user.Update(dto, resolvedProperties);

        await Repository.UpdateAsync(user);

        return (await GetByIdAsync(user!.Id))!;
    }

    private (List<UserProjectDto>?, List<UserProjectDto>?) GetProjects(UserUpdateDto dto, string userId)
    {
        var adminProjects = (dto.AssignedProjects ?? new List<string>())
            .Select(project => new UserProjectDto { ProjectId = project, UserId = userId }).ToList();

        var assignedProjects = (dto.UnderControlProjects ?? new List<string>())
            .Select(project => new UserProjectDto { ProjectId = project, UserId = userId }).ToList();

        if (!adminProjects.Any()) adminProjects = null;
        
        if (!assignedProjects.Any()) assignedProjects = null;

        return (adminProjects, assignedProjects);
    }
}
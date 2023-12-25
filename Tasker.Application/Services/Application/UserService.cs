using AutoMapper;
using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.DTOs.Application.User;
using Tasker.Application.Interfaces;
using Tasker.Application.Interfaces.Resolvers;
using Tasker.Application.Interfaces.Services;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Repositories;

namespace Tasker.Application.Services.Application;

public class UserService : EntityService<User, UserDto>, IUserService
{
    private readonly IUserResolver _resolver;
    private readonly IUserAuthService _userAuthService;
    
    public UserService(IEntityRepository<User> repository, IMapper mapper,
        IUserResolver resolver, IUserAuthService userAuthService) : base(repository, mapper)
    {
        _resolver = resolver;
        _userAuthService = userAuthService;
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

        await _resolver.ResolveAsync(
            admin: adminProjects,
            assigned: assignedProjects,
            userId: user.Id);
        
        return (await GetByIdAsync(user!.Id))!;
    }

    private (List<UserProjectDto>?, List<UserProjectDto>?) GetProjects(UserUpdateDto dto, string userId)
    {
        var assignedProjects = (dto.AssignedProjects ?? new List<string>())
            .Select(project => new UserProjectDto { ProjectId = project, UserId = userId }).ToList();

        var adminProjects = (dto.UnderControlProjects ?? new List<string>())
            .Select(project => new UserProjectDto { ProjectId = project, UserId = userId }).ToList();
        
        if (!assignedProjects.Any()) assignedProjects = null;

        if (!adminProjects.Any()) adminProjects = null;
        
        return (adminProjects, assignedProjects);
    }
    
    public new async Task<bool> DeleteAsync(string id)
    {
        var entity = await Repository.GetByIdAsync(id);
        
        if (entity is not null)
        {
            await _userAuthService.DeleteUserAsync(id);
            
            await Repository.DeleteAsync(entity);
            
            return true;
        }

        return false;
    }
}
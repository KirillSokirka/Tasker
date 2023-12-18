using Tasker.Application.DTOs.Application.ResolvedProperties;
using Tasker.Application.DTOs.Application.User;
using Tasker.Application.Interfaces.Resolvers;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Exceptions;
using Tasker.Domain.Repositories;

namespace Tasker.Application.Resolvers;

public class UserResolver : IUserResolver
{
    private readonly IEntityRepository<User> _repository;
    private readonly IProjectResolver _resolver;

    public UserResolver(IProjectResolver resolver, IEntityRepository<User> repository)
    {
        _resolver = resolver;
        _repository = repository;
    }

    public async Task<User> ResolveAsync(string id)
        => await _repository.GetByIdAsync(id)
           ?? throw new InvalidEntityException($"The user with {id} was not found");

    public async Task<UserResolvedPropertiesDto> ResolveAsync(UserUpdateDto dto)
        => new()
        {
            AssignedProjects = dto.AssignedProjects is not null
                ? await _resolver.ResolveAssignedProjectsAsync(p => p.UserId == dto.Id, dto.AssignedProjects)
                : null,
            UnderControlProjects = dto.UnderControlProjects is not null 
                ? await _resolver.ResolveAdminProjectsAsync(p => p.UserId == dto.Id, dto.UnderControlProjects)
                : null
        };
}
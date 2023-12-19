using Tasker.Application.DTOs.Application.Project;
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

    public async Task<UserResolvedPropertiesDto> ResolveAsync(List<UserProjectDto>? assigned,
        List<UserProjectDto>? admin, string userId)
        => new()
        {
            AssignedProjects = assigned is not null
                ? await _resolver.ResolveAssignedProjectsAsync(p => p.UserId == userId, assigned)
                : null,
            UnderControlProjects = admin is not null
                ? await _resolver.ResolveAdminProjectsAsync(p => p.UserId == userId, admin)
                : null
        };
}
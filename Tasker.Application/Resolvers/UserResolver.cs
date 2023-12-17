using Tasker.Application.DTOs.Application.ResolvedProperties;
using Tasker.Application.DTOs.Application.User;
using Tasker.Application.Interfaces.Resolvers;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Exceptions;
using Tasker.Domain.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Application.Resolvers;

public class UserResolver : IUserResolver
{
    private readonly IEntityRepository<User> _repository;
    private readonly IResolver<Project, string> _resolver;

    public UserResolver(IResolver<Project, string> resolver, IEntityRepository<User> repository)
    {
        _resolver = resolver;
        _repository = repository;
    }

    public async Task<User> ResolveAsync(string id)
        => await _repository.GetByIdAsync(id)
           ?? throw new InvalidEntityException($"The user with {id} was not found");

    public Task<UserResolvedPropertiesDto> ResolveAsync(UserUpdateDto dto)
        => Task.FromResult(new UserResolvedPropertiesDto
        {
            AssignedProjects = dto.AssignedProjects?
                .Select(project => _resolver.ResolveAsync(project).Result)
                .ToList(),
            UnderControlProjects = dto.UnderControlProjects?
                .Select(project => _resolver.ResolveAsync(project).Result)
                .ToList()
        });
}
using Microsoft.EntityFrameworkCore;
using Tasker.Application.DTOs.Application.User;
using Tasker.Application.Resolvers.DTOs;
using Tasker.Application.Resolvers.Interfaces;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Exceptions;
using Tasker.Infrastructure.Data.Application;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Application.Resolvers;

public class UserResolver : IUserResolver
{
    private readonly ApplicationContext _context;
    private readonly IResolver<Project, string> _resolver;

    public UserResolver(ApplicationContext context,
        IResolver<Project, string> resolver)
    {
        _context = context;
        _resolver = resolver;
    }

    public async Task<User> ResolveAsync(string id)
        => await _context.User.FirstOrDefaultAsync(u => u.Id == id)
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
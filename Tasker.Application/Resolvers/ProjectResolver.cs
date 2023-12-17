using Microsoft.EntityFrameworkCore;
using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.Interfaces.Resolvers;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Exceptions;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Application.Resolvers;

public class ProjectResolver : IResolver<Project, string>
{
    private readonly ApplicationContext _context;

    public ProjectResolver(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Project> ResolveAsync(string id)
        => await _context.Projects.FirstOrDefaultAsync(p => p.Id == id)
           ?? throw new InvalidEntityException($"Project with id {id} doesnt exists");
}
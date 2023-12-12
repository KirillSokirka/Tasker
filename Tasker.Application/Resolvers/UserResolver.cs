using Microsoft.EntityFrameworkCore;
using Tasker.Application.Resolvers.Interfaces;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Exceptions;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Application.Resolvers;

public class UserResolver : IResolver<User, string>
{
    private readonly ApplicationContext _context;

    public UserResolver(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<User> ResolveAsync(string id)
        => await _context.User.FirstOrDefaultAsync(u => u.Id == id) 
           ?? throw new InvalidEntityException($"The user with {id} was not found");
}
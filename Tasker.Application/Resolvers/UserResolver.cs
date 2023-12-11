using Microsoft.EntityFrameworkCore;
using Tasker.Application.DTOs.Application;
using Tasker.Application.Resolvers.Interfaces;
using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Application.Resolvers;

public class UserResolver : IResolver<User, UserDto>
{
    private readonly ApplicationContext _context;

    public UserResolver(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<User> ResolveAsync(UserDto dto)
        => await _context.User.FirstAsync(u => u.Id == dto.Id);
}
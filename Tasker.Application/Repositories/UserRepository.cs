using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tasker.Application.DTOs.Application.User;
using Tasker.Application.EntitiesExtension;
using Tasker.Application.Interfaces.Repositories;
using Tasker.Application.Resolvers.DTOs;
using Tasker.Application.Resolvers.Interfaces;
using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Application.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IResolver<UserResolvedPropertiesDto, UserUpdateDto> _resolver;
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public UserRepository(ApplicationContext context, IMapper mapper,
        IResolver<UserResolvedPropertiesDto, UserUpdateDto> resolver)
    {
        _resolver = resolver;
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto?> CreateAsync(UserDto dto)
    {
        var user = _mapper.Map<User>(dto);

        user.Id = Guid.NewGuid().ToString();

        await _context.User.AddAsync(user);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> UpdateAsync(UserUpdateDto dto)
    {
        var user = await _context.User.FindAsync(dto.Id);

        if (user is null)
        {
            return null;
        }

        var resolvedProperties = await _resolver.ResolveAsync(dto);

        user.Update(dto, resolvedProperties);
        
        await _context.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var user = await _context.User.FindAsync(id);

        if (user is null)
        {
            return false;
        }

        _context.User.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<UserDto?> GetAsync(string id)
    {
        var user = await _context.User.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        return user is not null ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<List<UserDto>> GetAllAsync() =>
        await _context.User
            .AsNoTracking()
            .Select(user => _mapper.Map<UserDto>(user))
            .ToListAsync();
}
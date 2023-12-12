using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tasker.Application.DTOs;
using Tasker.Application.DTOs.Application;
using Tasker.Application.Interfaces.Repositories;
using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Application.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public UserRepository(ApplicationContext context, IMapper mapper)
    {
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

    public async Task<UserDto?> UpdateAsync(UserDto dto)
    {
        var user = await _context.User.FindAsync(dto.Id);

        if (user is null)
        {
            return null;
        }

        _mapper.Map(dto, user);

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
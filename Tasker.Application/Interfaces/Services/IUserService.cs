using Tasker.Application.DTOs.Application.User;

namespace Tasker.Application.Interfaces;

public interface IUserService : IEntityService<UserDto>
{
    Task<UserDto> CreateAsync(UserDto createDto);
    Task<UserDto?> UpdateAsync(UserUpdateDto dto);
}
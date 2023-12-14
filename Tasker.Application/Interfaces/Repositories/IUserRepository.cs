using Tasker.Application.DTOs.Application;
using Tasker.Application.DTOs.Application.User;

namespace Tasker.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<UserDto?> CreateAsync(UserDto dto);
    Task<UserDto?> UpdateAsync(UserUpdateDto dto);
    Task<bool> DeleteAsync(string id);
    Task<UserDto?> GetAsync(string id);
    Task<List<UserDto>> GetAllAsync();
}
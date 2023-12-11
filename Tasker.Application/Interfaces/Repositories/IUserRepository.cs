using Tasker.Application.DTOs.Application;

namespace Tasker.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<UserDto?> CreateAsync(UserDto dto);
    Task<UserDto?> UpdateAsync(UserDto dto);
    Task<bool> DeleteAsync(string id);
    Task<UserDto?> GetAsync(string id);
    Task<List<UserDto>> GetAllAsync();
}
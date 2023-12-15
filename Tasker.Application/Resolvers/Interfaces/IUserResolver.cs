using Tasker.Application.DTOs.Application.User;
using Tasker.Application.Resolvers.DTOs;
using Tasker.Domain.Entities.Application;

namespace Tasker.Application.Resolvers.Interfaces;

public interface IUserResolver
{
    public Task<User> ResolveAsync(string id);

    Task<UserResolvedPropertiesDto> ResolveAsync(UserUpdateDto dto);
}
using Tasker.Application.DTOs.Application.Project;
using Tasker.Application.DTOs.Application.ResolvedProperties;
using Tasker.Application.DTOs.Application.User;
using Tasker.Domain.Entities.Application;

namespace Tasker.Application.Interfaces.Resolvers;

public interface IUserResolver : IResolver<User, string>
{
    Task<UserResolvedPropertiesDto> ResolveAsync(List<UserProjectDto>? assigned,
        List<UserProjectDto>? admin,
        string userId);
}
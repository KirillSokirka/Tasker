using Tasker.Application.DTOs.Application.ResolvedProperties;
using Tasker.Application.DTOs.Application.User;
using Tasker.Domain.Entities.Application;

namespace Tasker.Application.EntitiesExtension;

public static class UserExtension
{
    public static void Update(this User user, UserUpdateDto updateDto,
        UserResolvedPropertiesDto resolvedProperties)
    {
        user.Title = updateDto.Username ?? user.Title;

        user.AssignedProjects = resolvedProperties.AssignedProjects ?? user.AssignedProjects;
        user.UnderControlProjects = resolvedProperties.UnderControlProjects ?? user.UnderControlProjects;
    }
}
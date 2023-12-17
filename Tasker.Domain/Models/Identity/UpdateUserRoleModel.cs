namespace Tasker.Domain.Models.Identity;

public class UpdateUserRoleModel
{
    public required string Email { get; set; }
    public required List<string> Roles { get; set; }
}
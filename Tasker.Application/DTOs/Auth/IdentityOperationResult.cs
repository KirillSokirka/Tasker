namespace Tasker.Application.DTOs.Auth;

public class IdentityOperationResult
{
    public bool IsSuccess => Errors.Count == 0;
    public List<string> Errors { get; } = new();
    public TokenModel Token { get; set; } = null!;

    public void AddError(string error)
    {
        Errors.Add(error);
    }
}
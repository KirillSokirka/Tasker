namespace Tasker.Application.DTOs;

public class LoginOperationResult
{
    public bool IsSuccess => Errors.Count == 0;
    public List<string> Errors { get; } = new();
    public string Token { get; set; } = null!;

    public void AddError(string error)
    {
        Errors.Add(error);
    }
}
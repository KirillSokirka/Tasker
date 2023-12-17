using Tasker.Application.DTOs;

namespace Tasker.Domain.Models.Identity;

public class OperationResult
{
    public bool IsSuccess => Errors.Count == 0;
    public List<string> Errors { get; } = new();
    public TokenModel Token { get; set; } = null!;

    public void AddError(string error)
    {
        Errors.Add(error);
    }
}
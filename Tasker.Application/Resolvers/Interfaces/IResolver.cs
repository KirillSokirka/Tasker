namespace Tasker.Application.Resolvers.Interfaces;

public interface IResolver<TResult, in TInput>
{
    Task<TResult> ResolveAsync(TInput dto);
}
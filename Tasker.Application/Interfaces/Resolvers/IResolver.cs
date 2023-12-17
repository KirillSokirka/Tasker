namespace Tasker.Application.Interfaces.Resolvers;

public interface IResolver<TResult, in TInput>
{
    Task<TResult> ResolveAsync(TInput dto);
}
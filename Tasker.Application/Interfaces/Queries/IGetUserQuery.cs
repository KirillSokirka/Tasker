
using Microsoft.AspNetCore.Http;

namespace Tasker.Application.Interfaces.Queries
{
    public interface IGetUserQuery
    {
        public Task<string?> GetUserId(HttpContext context);
    }
}

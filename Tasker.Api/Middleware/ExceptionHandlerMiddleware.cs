using Microsoft.Extensions.Logging;
using Tasker.Application.Exceptions;

namespace Tasker.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _delegate;

        public ExceptionHandlerMiddleware(RequestDelegate delegateNext)
        {
            _delegate = delegateNext;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _delegate(context);
            }
            catch (InvalidEntityException exception)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var message = exception.Message;
                response.StatusCode = StatusCodes.Status408RequestTimeout;
                await response.WriteAsync(message);
            }
        }
    }
}

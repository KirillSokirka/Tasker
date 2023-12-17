using Tasker.Domain.Exceptions;

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
                await GenerateResponse(context, exception.Message);
            }
        }

        private static async Task GenerateResponse(HttpContext context, string message)
        {
            var response = context.Response;
            
            response.ContentType = "application/json";
            response.StatusCode = StatusCodes.Status500InternalServerError;
            
            await response.WriteAsync(message);
        }
    }
}
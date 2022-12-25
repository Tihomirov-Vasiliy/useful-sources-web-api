using System.Net;
using Domain.Exceptions;
using Domain.Entities;
using Services;

namespace WebApi.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            string responseMessage = null;

            context.Response.ContentType = "application/json";
            //add exception handling for saveChanges(return 500, and message)
            if (ex.GetType().Equals(typeof(WrongInputDataException)))
                context.Response.StatusCode = (int)((WrongInputDataException)ex).StatusCode;
            else if (ex.GetType().Equals(typeof(ObjectNotFoundException)))
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            else
            {
                context.Response.StatusCode = 500;
                responseMessage = ErrorMessages.SOMETHING_WENT_WRONG_IN_DATABASE;
            }


            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = responseMessage ??= ex.Message
            }.ToString());
        }
    }
}

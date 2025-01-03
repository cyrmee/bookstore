using Newtonsoft.Json;
using System.Net;

namespace Presentation.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.BadRequest;
        var message = exception.Message;

        statusCode = exception switch
        {
            NotFoundException => HttpStatusCode.NotFound,
            ValidationException => HttpStatusCode.BadRequest,
            _ => statusCode
        };

        var result = JsonConvert.SerializeObject(new { error = message });
        result = "";
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(result);
    }
}

public class NotFoundException(string message) : Exception(message)
{
}

public class ValidationException(string message) : Exception(message)
{
}

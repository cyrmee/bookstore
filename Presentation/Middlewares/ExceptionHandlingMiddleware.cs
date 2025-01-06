using Newtonsoft.Json;
using Serilog;
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
            Log.Error(ex.Message, "An error occurred while processing the request.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.BadRequest;
        var message = exception.Message;

        statusCode = exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            NotFoundException => HttpStatusCode.NotFound,
            BadRequestException => HttpStatusCode.BadRequest,
            ConflictException => HttpStatusCode.Conflict,
            UnprocessableEntityException => HttpStatusCode.UnprocessableEntity,
            UnauthorizedException => HttpStatusCode.Unauthorized,
            _ => statusCode
        };

        var result = JsonConvert.SerializeObject(new { error = message });
        // result = "";
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(result);
    }
}

public class UnauthorizedException(string message) : Exception(message)
{
}

public class UnprocessableEntityException(string message) : Exception(message)
{
}

public class ConflictException(string message) : Exception(message)
{
}

public class BadRequestException(string message) : Exception(message)
{
}

public class NotFoundException(string message) : Exception(message)
{
}

public class ValidationException(string message) : Exception(message)
{
}

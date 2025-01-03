using Serilog;
using System.Diagnostics;
using System.Text;

namespace Presentation.Middlewares;

public class LoggingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var originalBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;
        context.Request.EnableBuffering();
        string requestBody;
        using (var reader = new StreamReader(context.Request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true))
        {
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        await next(context);

        var request = context.Request;
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        var userAgent = request.Headers.UserAgent.ToString();
        var user = context.User.Identity?.Name;

        if (context.Request.Method is "PUT" or "PATCH" or "DELETE")
        {
            Log.Information("HTTP {RequestMethod} {RequestPath} received from {IpAddress} ({UserAgent}), " +
                "Username: {User}, Request Body: {RequestBody}",
                request.Method, request.Path, ipAddress, userAgent, user, requestBody);
        }
        else
        {
            Log.Information("HTTP {RequestMethod} {RequestPath} received from {IpAddress} ({UserAgent}), " +
                "Username: {User}",
                request.Method, request.Path, ipAddress, userAgent, user);
        }

        stopwatch.Stop();
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        await responseBody.CopyToAsync(originalBodyStream);
    }
}
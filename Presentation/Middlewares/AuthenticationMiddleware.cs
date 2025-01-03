using Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Middlewares;

public class AuthenticationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        var _authenticationService = context.RequestServices.GetRequiredService<IAuthenticationService>();
        var _configuration = context.RequestServices.GetRequiredService<IConfiguration>();

        var authorizeData = endpoint?.Metadata?.GetMetadata<IAuthorizeData>();
        var allowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>();

        if (authorizeData != null && allowAnonymous == null)
        {
            // The endpoint requires authorization
            if (!context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader) || string.IsNullOrEmpty(authorizationHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var token = authorizationHeader.ToString().Replace("Bearer ", "");

            // uncomment after implementing the IsTokenRevoked method using redis

            //if (await _authenticationService.IsTokenRevoked(token))
            //{
            //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //    return;
            //}
        }

        await _next(context);
    }
}


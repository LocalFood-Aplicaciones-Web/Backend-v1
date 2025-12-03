using Backend.API.IAM.Application.Internal.OutboundServices;
using Backend.API.IAM.Domain.Model.Queries;
using Backend.API.IAM.Domain.Services;
using Backend.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace Backend.API.IAM.Infrastructure.Pipeline.Middleware.Components;

/**
 * RequestAuthorizationMiddleware is a custom middleware.
 * This middleware is used to authorize requests.
 * It validates a token is included in the request header and that the token is valid.
 * If the token is valid then it sets the user in HttpContext.Items["User"].
 */
public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    /**
     * InvokeAsync is called by the ASP.NET Core runtime.
     * It is used to authorize requests.
     * It validates a token is included in the request header and that the token is valid.
     * If the token is valid then it sets the user in HttpContext.Items["User"].
     */
    public async Task InvokeAsync(
        HttpContext context,
        IUserQueryService userQueryService,
        ITokenService tokenService)
    {
        try
        {
            // skip authorization if endpoint is decorated with [AllowAnonymous] attribute
            var endpoint = context.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata.Any(m => 
                m.GetType() == typeof(Backend.API.IAM.Infrastructure.Pipeline.Middleware.Attributes.AllowAnonymousAttribute) || 
                m.GetType() == typeof(Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute)) ?? false;
            
            if (allowAnonymous)
            {
                // [AllowAnonymous] attribute is set, so skip authorization
                await next(context);
                return;
            }
            
            // get token from request header
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            
            if (string.IsNullOrEmpty(authHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Missing Authorization header");
                return;
            }

            var token = authHeader.Split(" ").Last();

            // if token is null or empty then return 401
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid token format");
                return;
            }

            // validate token
            var userId = await tokenService.ValidateToken(token);

            // if token is invalid then return 401
            if (userId == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid or expired token");
                return;
            }

            // get user by id
            var getUserByIdQuery = new GetUserByIdQuery(userId.Value);
            var user = await userQueryService.Handle(getUserByIdQuery);
            
            if (user == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("User not found");
                return;
            }
            
            // set user in HttpContext.Items for downstream middleware/handlers
            context.Items["User"] = user;
            
            // call next middleware
            await next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Authorization error: {ex.Message}");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync($"Authorization failed: {ex.Message}");
        }
    }
}
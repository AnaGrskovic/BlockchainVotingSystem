using System.Net;
using System.Text.Json;
using VotingApp.Contracts.Exceptions;

namespace VotingApp.API.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await OnException(context, ex);
        }
    }

    public async Task OnException(HttpContext context, Exception ex)
    {
        object response;
        _ = ex switch
        {
            TokenNotPresentException or
            TokenNotValidException =>
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized,
            _ => context.Response.StatusCode = (int)HttpStatusCode.InternalServerError
        };

        response = new
        {
            ex.Message
        };

        context.Response.ContentType = "application/json";

        var resultJson = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(resultJson);
    }
}

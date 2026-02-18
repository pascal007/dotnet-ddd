using System.Text.Json;
using WalletDemo.Application.Common; // where ApiResponse lives
using WalletDemo.Domain.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<string>.Fail(ex.Message);

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception e)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<string>.Fail("An unexpected error occurred." + e.Message);

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

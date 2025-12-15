using CandidatesChannels.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CandidatesChannels.WebApi.Middleware;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain error");
            await WriteProblem(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error");
            await WriteProblem(context, StatusCodes.Status500InternalServerError, "Unexpected error.");
        }
    }

    private static async Task WriteProblem(HttpContext context, int statusCode, string detail)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = statusCode == 400 ? "Validation error" : "Server error",
            Detail = detail
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
}

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Exceptions;

namespace ProjectManagement.API.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
private readonly ILogger<GlobalExceptionHandler> _logger;


public GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger)
{
    _logger = logger;
}

public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken)
{
    _logger.LogError(
        exception,
        "An unhandled exception occurred.");

    var statusCode = exception switch
    {
        TaskValidationException =>
            StatusCodes.Status400BadRequest,

        ProjectValidationException =>
            StatusCodes.Status400BadRequest,

        ProjectMemberValidationException =>
            StatusCodes.Status400BadRequest,

        KeyNotFoundException =>
            StatusCodes.Status404NotFound,

        InvalidOperationException =>
            StatusCodes.Status409Conflict,

        _ =>
            StatusCodes.Status500InternalServerError
    };

    var title = exception switch
    {
        TaskValidationException =>
            "Task validation failed.",

        ProjectValidationException =>
            "Project validation failed.",

        ProjectMemberValidationException =>
            "Project member validation failed.",

        KeyNotFoundException =>
            "Resource not found.",

        InvalidOperationException =>
            "Operation could not be completed.",

        _ =>
            "An unexpected error occurred."
    };

    var detail = statusCode == StatusCodes.Status500InternalServerError
        ? "An internal server error occurred."
        : exception.Message;

    var problemDetails = new ProblemDetails
    {
        Status = statusCode,
        Title = title,
        Detail = detail
    };

    httpContext.Response.StatusCode = statusCode;

    await httpContext.Response.WriteAsJsonAsync(
        problemDetails,
        cancellationToken);

    return true;
}


}

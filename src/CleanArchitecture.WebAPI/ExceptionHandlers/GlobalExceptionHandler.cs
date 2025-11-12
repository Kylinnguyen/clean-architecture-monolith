using System.Text.Json;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Constants;
using CleanArchitecture.WebAPI.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebAPI.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _hostEnvironment;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unexpected error occurred.");

        var (statusCode, message, errorCode) = exception switch
        {
            NotFoundException ex => (StatusCodes.Status404NotFound, ex.Message, ex.ErrorCode),
            UnauthorizedException ex => (StatusCodes.Status401Unauthorized, ex.Message, ex.ErrorCode),
            ForbiddenAccessException ex => (StatusCodes.Status403Forbidden, ex.Message, ex.ErrorCode),
            BadRequestException ex => (StatusCodes.Status400BadRequest, ex.Message, ex.ErrorCode),
            BusinessException ex => (StatusCodes.Status400BadRequest, ex.Message, ex.ErrorCode),
            InternalServerErrorException ex => (StatusCodes.Status500InternalServerError, ex.Message, ex.ErrorCode),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.", ErrorCodeConstants.INTERNAL_SERVER_ERROR)
        };
        
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var response = _hostEnvironment.IsDevelopment()
            ? ApiResult.Fail(message, null, statusCode)
            : ApiResult.Fail("Internal server error");
        
        var result = JsonSerializer.Serialize(response, jsonOptions);
        await httpContext.Response.WriteAsync(result, cancellationToken);
        return true;
    }
}
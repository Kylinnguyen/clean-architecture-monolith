// using System.Text.Json;
// using CleanArchitecture.Application.Common.Exceptions;
// using Microsoft.AspNetCore.Diagnostics;
// using Microsoft.AspNetCore.Mvc;
//
// namespace CleanArchitecture.WebAPI.ExceptionHandlers;
//
// public class GlobalExceptionHandlerBak : IExceptionHandler
// {
//     private readonly ILogger<GlobalExceptionHandler> _logger;
//
//     public GlobalExceptionHandlerBak(ILogger<GlobalExceptionHandler> logger, IHostEnvironment hostEnvironment)
//     {
//         _logger = logger;
//     }
//
//     public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
//         CancellationToken cancellationToken)
//     {
//         _logger.LogError(exception, "An unexpected error occurred.");
//
//         switch (exception)
//         {
//             case UnauthorizedException:
//                 await HandleUnauthorizedException(httpContext, exception, cancellationToken);
//                 break;
//             case ForbiddenAccessException:
//                 await HandleForbiddenException(httpContext, exception, cancellationToken);
//                 break;
//             case NotFoundException:
//                 await HandleNotFoundException(httpContext, exception, cancellationToken);
//                 break;
//             case BadRequestException:
//                 await HandleBadRequestException(httpContext, exception, cancellationToken);
//                 break;
//             default:
//                 await HandleInternalServerException(httpContext, exception, cancellationToken);
//                 break;
//         }
//
//         return true;
//     }
//
//     private async Task HandleUnauthorizedException(HttpContext httpContext, Exception exception,
//         CancellationToken cancellationToken = default)
//     {
//         httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
//         httpContext.Response.ContentType = "application/json";
//
//         var problemDetails = new ProblemDetails()
//         {
//             Title = "Unauthorized",
//             Status = StatusCodes.Status401Unauthorized,
//             Instance = httpContext.Request.Path,
//             Detail = exception.Message,
//         };
//         problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);
//         problemDetails.Extensions.Add("message", exception.Message);
//
//         var jsonOptions = new JsonSerializerOptions
//         {
//             PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//         };
//
//         var result = JsonSerializer.Serialize(problemDetails, jsonOptions);
//         await httpContext.Response.WriteAsync(result, cancellationToken);
//     }
//
//     private static async Task HandleForbiddenException(HttpContext httpContext, Exception exception,
//         CancellationToken cancellationToken = default)
//     {
//         httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
//         httpContext.Response.ContentType = "application/json";
//
//         var problemDetails = new ProblemDetails()
//         {
//             Title = "Forbidden",
//             Status = StatusCodes.Status403Forbidden,
//             Instance = httpContext.Request.Path,
//             Detail = exception.Message,
//         };
//         problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);
//         problemDetails.Extensions.Add("message", exception.Message);
//
//         var jsonOptions = new JsonSerializerOptions
//         {
//             PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//         };
//
//         var result = JsonSerializer.Serialize(problemDetails, jsonOptions);
//         await httpContext.Response.WriteAsync(result, cancellationToken);
//     }
//
//     private static async Task HandleNotFoundException(HttpContext httpContext, Exception exception,
//         CancellationToken cancellationToken = default)
//     {
//         httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
//         httpContext.Response.ContentType = "application/json";
//
//         var problemDetails = new ProblemDetails()
//         {
//             Title = "Not found",
//             Status = StatusCodes.Status404NotFound,
//             Instance = httpContext.Request.Path,
//             Detail = exception.Message,
//         };
//         problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);
//         problemDetails.Extensions.Add("message", exception.Message);
//
//         var jsonOptions = new JsonSerializerOptions
//         {
//             PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//         };
//
//         var result = JsonSerializer.Serialize(problemDetails, jsonOptions);
//         await httpContext.Response.WriteAsync(result, cancellationToken);
//     }
//
//     private static async Task HandleBadRequestException(HttpContext httpContext, Exception exception,
//         CancellationToken cancellationToken = default)
//     {
//         httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
//         httpContext.Response.ContentType = "application/json";
//
//         var problemDetails = new ProblemDetails()
//         {
//             Title = "Bad request",
//             Status = StatusCodes.Status400BadRequest,
//             Instance = httpContext.Request.Path,
//             Detail = exception.Message,
//         };
//         problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);
//         problemDetails.Extensions.Add("message", exception.Message);
//
//         var jsonOptions = new JsonSerializerOptions
//         {
//             PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//         };
//
//         var result = JsonSerializer.Serialize(problemDetails, jsonOptions);
//         await httpContext.Response.WriteAsync(result, cancellationToken);
//     }
//
//     private static async Task HandleInternalServerException(HttpContext httpContext, Exception exception,
//         CancellationToken cancellationToken = default)
//     {
//         httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
//         httpContext.Response.ContentType = "application/json";
//
//         var problemDetails = new ProblemDetails()
//         {
//             Title = "Internal Server Error",
//             Status = StatusCodes.Status500InternalServerError,
//             Instance = httpContext.Request.Path,
//             Detail = exception.Message,
//         };
//         problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);
//         problemDetails.Extensions.Add("message", exception.Message);
//
//         var jsonOptions = new JsonSerializerOptions
//         {
//             PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//         };
//
//         var result = JsonSerializer.Serialize(problemDetails, jsonOptions);
//         await httpContext.Response.WriteAsync(result, cancellationToken);
//     }
// }
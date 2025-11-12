// using System.Net;
// using System.Text.Json;
// using CleanArchitecture.WebAPI.Common;
// using CleanArchitecture.Application.Common.Exceptions;
// using CleanArchitecture.Application.Constants;
//
// namespace CleanArchitecture.WebAPI.Middlewares;
//
// public class GlobalExceptionMiddleware
// {
//     private readonly RequestDelegate _next;
//     private readonly ILogger<GlobalExceptionMiddleware> _logger;
//     private readonly IWebHostEnvironment _environment;
//
//     public GlobalExceptionMiddleware(
//         RequestDelegate next,
//         ILogger<GlobalExceptionMiddleware> logger,
//         IWebHostEnvironment environment)
//     {
//         _next = next;
//         _logger = logger;
//         _environment = environment;
//     }
//
//     public async Task InvokeAsync(HttpContext context)
//     {
//         try
//         {
//             await _next(context);
//         }
//         catch (Exception ex)
//         {
//             await HandleExceptionAsync(context, ex);
//         }
//     }
//
//     private Task HandleExceptionAsync(HttpContext context, Exception exception)
//     {
//         var response = context.Response;
//         response.ContentType = "application/json";
//
//         var traceId = context.TraceIdentifier;
//         var path = context.Request.Path;
//         var method = context.Request.Method;
//         var timestamp = DateTime.UtcNow;
//
//         // Log exception with appropriate level and details
//         LogException(exception, path, method, traceId);
//
//         ResponseResult responseResult = null;
//         string errorCode;
//
//         switch (exception)
//         {
//             case NotFoundException notFoundEx:
//                 responseResult.StatusCode = (int)HttpStatusCode.NotFound;
//                 errorCode = ErrorCodeConstants.ENTITY_NOT_FOUND;
//                 responseResult = ResponseResult.Fail(
//                     notFoundEx.Message ?? "Resource not found.",
//                     BuildErrorObject(errorCode, traceId, timestamp, path, new
//                     {
//                         EntityName = notFoundEx.EntityName,
//                         EntityId = notFoundEx.EntityId
//                     }),
//                     responseResult.StatusCode);
//                 break;
//
//             case UnauthorizedException unauthorizedEx:
//                 responseResult.StatusCode = (int)HttpStatusCode.Unauthorized;
//                 errorCode = ErrorCodeConstants.UNAUTHORIZED;
//                 responseResult = ResponseResult.Fail(
//                     unauthorizedEx.Message ?? "Authentication required.",
//                     BuildErrorObject(errorCode, traceId, timestamp, path),
//                     responseResult.StatusCode);
//                 break;
//
//             case ForbiddenException forbiddenEx:
//                 responseResult.StatusCode = (int)HttpStatusCode.Forbidden;
//                 errorCode = ErrorCodeConstants.FORBIDDEN;
//                 responseResult = ResponseResult.Fail(
//                     forbiddenEx.Message ?? "You do not have permission to perform this action.",
//                     BuildErrorObject(errorCode, traceId, timestamp, path, new
//                     {
//                         Resource = forbiddenEx.Resource,
//                         Action = forbiddenEx.Action
//                     }),
//                     responseResult.StatusCode);
//                 break;
//
//             case BadRequestException badRequestEx:
//                 responseResult.StatusCode = (int)HttpStatusCode.BadRequest;
//                 errorCode = ErrorCodeConstants.BAD_REQUEST;
//                 responseResult = ResponseResult.Fail(
//                     badRequestEx.Message ?? "Invalid request.",
//                     BuildErrorObject(errorCode, traceId, timestamp, path, badRequestEx.Errors),
//                     responseResult.StatusCode);
//                 break;
//
//             default:
//                 responseResult.StatusCode = (int)HttpStatusCode.InternalServerError;
//                 errorCode = "INTERNAL_SERVER_ERROR";
//                 var errorDetails = BuildErrorObject(errorCode, traceId, timestamp, path, null);
//
//                 // Include stack trace in development environment only
//                 if (_environment.IsDevelopment())
//                 {
//                     errorDetails = new
//                     {
//                         ErrorCode = errorCode,
//                         Timestamp = timestamp,
//                         TraceId = traceId,
//                         Path = path,
//                         StackTrace = exception.StackTrace
//                     };
//                 }
//
//                 responseResult = ResponseResult.Fail(
//                     "An error occurred while processing your request.",
//                     errorDetails,
//                     responseResult.StatusCode);
//                 break;
//         }
//
//         var jsonOptions = new JsonSerializerOptions
//         {
//             PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//         };
//
//         var jsonResponse = JsonSerializer.Serialize(responseResult, jsonOptions);
//         return response.WriteAsync(jsonResponse);
//     }
//
//     private void LogException(Exception exception, string path, string method, string traceId)
//     {
//         var logLevel = exception switch
//         {
//             NotFoundException => LogLevel.Warning,
//             UnauthorizedException => LogLevel.Warning,
//             ForbiddenException => LogLevel.Warning,
//             BadRequestException => LogLevel.Warning,
//             _ => LogLevel.Error
//         };
//
//         _logger.Log(logLevel, exception,
//             "Exception occurred: {ExceptionType} | Path: {Path} | Method: {Method} | TraceId: {TraceId} | Message: {Message}",
//             exception.GetType().Name, path, method, traceId, exception.Message);
//     }
//
//     private object BuildErrorObject(string errorCode, string traceId, DateTime timestamp, string path, object? additionalData = null)
//     {
//         var errorObject = new
//         {
//             ErrorCode = errorCode,
//             Timestamp = timestamp,
//             TraceId = traceId,
//             Path = path
//         };
//
//         if (additionalData != null)
//         {
//             // Merge additional data with base error object
//             return new Dictionary<string, object?>
//             {
//                 { "errorCode", errorCode },
//                 { "timestamp", timestamp },
//                 { "traceId", traceId },
//                 { "path", path },
//                 { "details", additionalData }
//             };
//         }
//
//         return errorObject;
//     }
// }
//

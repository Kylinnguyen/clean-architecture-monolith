using System.Text.Json;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.WebAPI.Common;
using FluentValidation;

namespace CleanArchitecture.WebAPI.Middlewares;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _hostEnvironment;

    public ValidationExceptionMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment)
    {
        _next = next;
        _hostEnvironment = hostEnvironment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationCustomException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors
                .GroupBy(x => x.Key)
                .ToDictionary(x => x.Key.ToLower(),
                    x => x.First().Value);

            var response = _hostEnvironment.IsDevelopment()
                ? ApiResult.Fail(ex.Message, errors, StatusCodes.Status400BadRequest)
                : ApiResult.Fail("Internal server error");

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var result = JsonSerializer.Serialize(response, jsonOptions);
            await context.Response.WriteAsync(result);
        }
    }
}
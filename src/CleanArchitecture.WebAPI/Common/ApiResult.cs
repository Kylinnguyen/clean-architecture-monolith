namespace CleanArchitecture.WebAPI.Common;

public class ApiResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public object? Errors { get; set; }
    public int StatusCode { get; set; }

    protected ApiResult(bool success, string? message, object? errors = null, 
        int statusCode = StatusCodes.Status200OK)
    {
        Message = message;
        Errors = errors;
        StatusCode = statusCode;
        Success = success;
    }

    public static ApiResult Ok(string? message = null) => new(true, message);

    public static ApiResult Fail(string? message = null, object? errors = null,
        int statusCode = StatusCodes.Status500InternalServerError)
    {
        return new ApiResult(false, message, errors, statusCode);
    }
}

public class ApiResult<TResult> : ApiResult where TResult : class
{
    public TResult? Result { get; set; }

    public ApiResult(
        bool success,
        string? message,
        TResult? result,
        object? error = null,
        int statusCode = StatusCodes.Status200OK)
        : base(success, message, error, statusCode)
    {
        Result = result;
    }

    public static ApiResult<TResult> Ok(TResult result, string? message = null)
    {
        return new ApiResult<TResult>(true, message, result);
    }

    public static ApiResult<TResult> Fail(
        TResult? result,
        string message,
        object? errors = null,
        int statusCode = StatusCodes.Status500InternalServerError)
    {
        return new ApiResult<TResult>(false, message, result, errors, statusCode);
    }
}
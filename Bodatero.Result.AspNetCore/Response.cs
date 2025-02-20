namespace Bodatero.Result.AspNetCore
{
    public class OkResult
    {
        public OkResult(object? data)
        {
            Data = data;
        }

        public OkResult(object? data, int statusCode = 200, string? message = null)
        {
            Data = data;
            StatusCode = statusCode;
            Message = message;
        }

        public int StatusCode { get; set; }
        public object? Data { get; set; }
        public string? Message { get; set; } = string.Empty;
    }

    public class BadResult
    {
        public BadResult(string message, int statusCode = 400, object? errorData = null)
        {
            StatusCode = statusCode;
            Message = message;
            ErrorData = errorData;
        }

        public BadResult(Exception exception, int statusCode = 400, object? errorData = null)
        {
            StatusCode = statusCode;
            Message = exception.Message;
            ErrorData = errorData;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object? ErrorData { get; set; }
    }
}

using System.Collections.Immutable;

namespace Bodatero.Result;

public class Result<TValue>
{
    public Result(TValue value)
    {
        IsSuccess = true;
        Value = value;
    }

    public Result(Exception exception)
    {
        IsSuccess = false;
        Error = exception;
    }

    public TValue? Value { get; init; }
    public bool IsSuccess { get; init; }
    public bool IsError => !IsSuccess;
    public Exception? Error { get; init; } 

    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Exception exception) => new(exception);

    public Result<TValue> Match(Func<TValue, Result<TValue>> success, Func<Exception, Result<TValue>> failure)
    {
        if (IsSuccess)
        {
            return success(Value!);
        }

        return failure(Error!);
    }
}
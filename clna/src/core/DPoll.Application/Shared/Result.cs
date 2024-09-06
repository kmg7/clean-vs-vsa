using DPoll.Domain.Common.Errors;

namespace DPoll.Application.Shared;

public class Result<T>
{
    public bool IsSuccess { get; }
    public Error Error { get; }
    public readonly T? Value;

    private Result(bool isSuccess, Error err, T? val)
    {
        IsSuccess = isSuccess;
        Error = err;
        Value = val;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, Error.None, value);
    }

    public static Result<T> Failure(Error error, T? value = default)
    {
        return new Result<T>(false, error, value);
    }
}


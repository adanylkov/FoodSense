namespace FoodSense.Common.Models;

public record Result
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }

    public static Result Success() => new Result { IsSuccess = true };
    public static Result Failure(string errorMessage) => new Result { IsSuccess = false, ErrorMessage = errorMessage };
}

public record Result<T>
{
    public T? Data { get; init; }
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }

    public static Result<T> Success(T data) => new Result<T> { Data = data, IsSuccess = true };
    public static Result<T> Failure(string errorMessage) => new Result<T> { IsSuccess = false, ErrorMessage = errorMessage };
}
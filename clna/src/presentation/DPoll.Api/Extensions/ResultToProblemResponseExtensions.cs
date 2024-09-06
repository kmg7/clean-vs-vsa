using DPoll.Application.Shared;
using DPoll.Domain.Common.Errors;

namespace DPoll.Api.Extensions;

public static class ResultToProblemResponseExtensions
{
    public static IResult ProblemResponse<T>(this Result<T> result)
    {
        var error = result.Error;
        var problemResponse = error.Code switch
        {
            ErrorCodes.NotFound => NotFound404Response(result),
            _ => InternalServerError500Response(result)
        };
        return problemResponse;
    }

    public static IResult BadRequest400Response<T>(this Result<T> result)
    {
        return Results.Problem(
            title: result.Error.Code,
            detail: result.Error.Description,
            statusCode: StatusCodes.Status400BadRequest);
    }

    public static IResult NotFound404Response<T>(this Result<T> result)
    {
        return Results.Problem(
            title: result.Error.Code,
            detail: result.Error.Description,
            statusCode: StatusCodes.Status404NotFound);
    }

    public static IResult InternalServerError500Response<T>(this Result<T> result)
    {
        return Results.Problem(
            title: result.Error.Code,
            detail: result.Error.Description,
            statusCode: StatusCodes.Status500InternalServerError);
    }
}


using DPoll.Application.Shared;
using DPoll.Domain.Common.Errors;

namespace DPoll.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResponse<T>(this Result<T> result)
    {
        if (result == null)
            return NoContent204Response(result);

        if (result.IsSuccess)
            return Results.Ok(result.Value);

        var error = result.Error;
        var problemResponse = error.Code switch
        {
            ErrorCodes.NotFound => NotFound404Response(result),
            _ => InternalServerError500Response(result)
        };
        return problemResponse;
    }
    
    public static IResult Ok200Response<T>(this Result<T> result)
    {
        return Results.Ok<T>(result.Value);
    }

    public static IResult Created201Response<T>(this Result<T> result, string uri = null)
    {
        return Results.Created<T>(uri, result.Value);
    }

    public static IResult NoContent204Response<T>(this Result<T> _)
    {
        return Results.NoContent();
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


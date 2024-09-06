using DPoll.Application.Shared;

namespace DPoll.Api.Extensions;

public static class ResultToResponseExtensions
{
    public static IResult Ok200Response<T>(this Result<T> result)
    {
        if (!result.IsSuccess)
            return result.ProblemResponse();

        return Results.Ok<T>(result.Value);
    }

    public static IResult NoContent204Response<T>(this Result<T> result)
    {
        if (!result.IsSuccess)
            return result.ProblemResponse();

        return Results.NoContent();
    }

    public static IResult Created201Response<T>(this Result<T> result, string uri = null)
    {
        if (!result.IsSuccess)
            return result.ProblemResponse();

        return Results.Created<T>(uri, result.Value);
    }

}


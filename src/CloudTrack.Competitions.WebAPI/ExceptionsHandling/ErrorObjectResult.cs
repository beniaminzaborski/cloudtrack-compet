using CloudTrack.Competitions.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CloudTrack.Competitions.WebAPI.ExceptionsHandling;

internal class ErrorObjectResult : ObjectResult
{
    public ErrorObjectResult(object error, int statusCode)
                : base(error)
    {
        StatusCode = statusCode;
    }

    public static ErrorObjectResult Create(Exception exception)
    {
        var message = exception.Message;
        var httpStatusCode = (int)HttpStatusCode.InternalServerError;

        switch (exception)
        {
            case ValidationException vex:
                httpStatusCode = (int)HttpStatusCode.BadRequest;
                message = string.Join(". ", vex.ValidationErrors.Select(x => $"{x.PropertyName}: {x.ErrorMessage}").ToArray());
                break;
            case FluentValidation.ValidationException:
                httpStatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case NotFoundException:
                httpStatusCode = (int)HttpStatusCode.NotFound;
                break;
            case InvalidOperationException:
                httpStatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        return new ErrorObjectResult(new ErrorResponseDto(message), httpStatusCode);
    }
}

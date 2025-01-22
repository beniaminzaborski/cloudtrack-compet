namespace CloudTrack.Competitions.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public IEnumerable<ValidationError> ValidationErrors { get; }

    public ValidationException(IEnumerable<ValidationError> validationErrors) 
    { 
        ValidationErrors = validationErrors;
    }

    public ValidationException(ValidationError validationError)
    {
        ValidationErrors = new[] { validationError };
    }

    public ValidationException(string propertyName, string validationErrorMessage)
    {
        ValidationErrors = new[] { new ValidationError(propertyName, validationErrorMessage) };
    }

    public ValidationException(string validationErrorMessage)
    {
        ValidationErrors = new[] { new ValidationError(string.Empty, validationErrorMessage) };
    }
}

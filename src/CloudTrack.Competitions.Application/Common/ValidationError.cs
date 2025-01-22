namespace CloudTrack.Competitions.Application.Common;

public sealed record ValidationError(
    string PropertyName,
    string ErrorMessage) { }

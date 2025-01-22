using CloudTrack.Competitions.Application.Competitions.Commands;
using FluentValidation;

namespace CloudTrack.Competitions.Application.Competitions;

public class AddCheckpointRequestCommandValidator : AbstractValidator<AddCheckpointRequestCommand>
{
    public AddCheckpointRequestCommandValidator()
    {
        RuleFor(x => x.TrackPointAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Must be greater than or equal to 0");

        RuleFor(x => x.TrackPointUnit)
            .NotEmpty().WithMessage("Must be not empty");
    }
}

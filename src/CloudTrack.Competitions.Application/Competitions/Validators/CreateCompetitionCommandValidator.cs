using CloudTrack.Competitions.Application.Competitions.Commands;
using FluentValidation;

namespace CloudTrack.Competitions.Application.Competitions;

public class CreateCompetitionCommandValidator : AbstractValidator<CreateCompetitionCommand>
{
    public CreateCompetitionCommandValidator(
        IValidator<DistanceDto> distanceValidator,
        IValidator<CompetitionPlaceDto> placeValidator)
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Field is required")
            .MaximumLength(250).WithMessage("Max lenght is 250 characters");

        RuleFor(x => x.StartAt)
          .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Cannot be in the past");

        RuleFor(x => x.MaxCompetitors)
            .GreaterThan(0).WithMessage("Greater than 0");

        RuleFor(x => x.Distance)
            .NotNull().WithMessage("Field is required")
            .SetValidator(distanceValidator);

        RuleFor(x => x.Place)
            .NotNull().WithMessage("Field is required")
            .SetValidator(placeValidator);
    }
}

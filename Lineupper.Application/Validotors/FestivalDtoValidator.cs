using FluentValidation;
using Lineupper.Application.Dto;

namespace Lineupper.Application.Validators
{
    public class FestivalDtoValidator : AbstractValidator<FestivalDto>
    {
        public FestivalDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Festival name is required");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required");

            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate).WithMessage("StartDate must be before EndDate");

            RuleFor(x => x.ConcertStartTime)
                .LessThan(x => x.ConcertEndTime).WithMessage("ConcertStartTime must be before ConcertEndTime");

            RuleFor(x => x.Bands)
                .NotNull().WithMessage("Bands list cannot be null");
        }
    }
}

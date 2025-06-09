using FluentValidation;
using Lineupper.Application.Dto;

namespace Lineupper.Application.Validators
{
    public class ScheduleItemDtoValidator : AbstractValidator<ScheduleItemDto>
    {
        public ScheduleItemDtoValidator()
        {
            RuleFor(x => x.FestivalId)
                .NotEmpty().WithMessage("FestivalId is required");

            RuleFor(x => x.BandId)
                .NotEmpty().WithMessage("BandId is required");

            RuleFor(x => x.StartTime)
                .LessThan(x => x.EndTime).WithMessage("StartTime must be before EndTime");
        }
    }
}

using FluentValidation;
using Lineupper.Application.Dto;

namespace Lineupper.Application.Validators
{
    public class BandDtoValidator : AbstractValidator<BandDto>
    {
        public BandDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Band name is required");

            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Genre is required");

            RuleFor(x => x.SetDuration)
                .GreaterThan(0).WithMessage("SetDuration must be positive");
        }
    }
}

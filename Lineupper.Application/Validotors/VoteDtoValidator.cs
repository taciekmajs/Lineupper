using FluentValidation;
using Lineupper.Application.Dto;

namespace Lineupper.Application.Validators
{
    public class VoteDtoValidator : AbstractValidator<VoteDto>
    {
        public VoteDtoValidator()
        {
            RuleFor(x => x.ParticipantId)
                .NotEmpty().WithMessage("ParticipantId is required");

            RuleFor(x => x.BandId)
                .NotEmpty().WithMessage("BandId is required");

            RuleFor(x => x.VoteValue)
                .InclusiveBetween(1, 5).WithMessage("VoteValue must be between 1 and 5");
        }
    }
}

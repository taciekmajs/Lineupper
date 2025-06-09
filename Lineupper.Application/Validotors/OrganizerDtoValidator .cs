using FluentValidation;
using Lineupper.Application.Dto;

namespace Lineupper.Application.Validators
{
    public class OrganizerDtoValidator : AbstractValidator<OrganizerDto>
    {
        public OrganizerDtoValidator()
        {
            Include(new UserDtoValidator());
        }
    }
}

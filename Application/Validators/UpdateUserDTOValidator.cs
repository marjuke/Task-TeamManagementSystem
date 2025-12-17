using Domain.DTOs;
using FluentValidation;

namespace Application.Validators
{
    public class UpdateUserDTOValidator : AbstractValidator<UpdateUserDTO>
    {
        public UpdateUserDTOValidator()
        {
            RuleFor(x => x.DisplayName)
                .MinimumLength(2).When(x => !string.IsNullOrEmpty(x.DisplayName))
                .WithMessage("Display name must be at least 2 characters long.")
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.DisplayName))
                .WithMessage("Display name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Email must be a valid email address.")
                .MaximumLength(256).When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Email cannot exceed 256 characters.");
        }
    }
}

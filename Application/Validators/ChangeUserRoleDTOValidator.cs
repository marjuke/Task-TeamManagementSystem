using Domain.DTOs;
using FluentValidation;

namespace Application.Validators
{
    public class ChangeUserRoleDTOValidator : AbstractValidator<ChangeUserRoleDTO>
    {
        private readonly List<string> _validRoles = new() { "Admin", "Manager", "Employee" };

        public ChangeUserRoleDTOValidator()
        {
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(role => _validRoles.Contains(role))
                .WithMessage($"Role must be one of: {string.Join(", ", _validRoles)}");
        }
    }
}

using Application.Features.WorkTasks.Commands;
using FluentValidation;

namespace Application.Validators
{
    public class UpdateTaskStatusCommandValidator : AbstractValidator<UpdateTaskStatusCommand>
    {
        public UpdateTaskStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Task ID must be greater than 0.");

            RuleFor(x => x.StatusID)
                .GreaterThan(0).WithMessage("Status ID must be greater than 0.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.")
                .NotNull().WithMessage("User ID cannot be null.");
        }
    }
}

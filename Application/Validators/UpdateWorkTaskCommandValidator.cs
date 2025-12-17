using Application.Features.WorkTasks.Commands;
using FluentValidation;

namespace Application.Validators
{
    public class UpdateWorkTaskCommandValidator : AbstractValidator<UpdateWorkTaskCommand>
    {
        public UpdateWorkTaskCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Task ID must be greater than 0.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(x => x.StatusID)
                .GreaterThan(0).WithMessage("Status ID must be greater than 0.");

            RuleFor(x => x.TeamId)
                .GreaterThan(0).WithMessage("Team ID must be greater than 0.");

            RuleFor(x => x.AssignedToUserID)
                .NotEmpty().WithMessage("Assigned To User ID is required.")
                .NotNull().WithMessage("Assigned To User ID cannot be null.");

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.MinValue).WithMessage("Due date must be valid.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Due date cannot be in the past.");
        }
    }
}

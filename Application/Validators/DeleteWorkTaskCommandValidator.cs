using Application.Features.WorkTasks.Commands;
using FluentValidation;

namespace Application.Validators
{
    public class DeleteWorkTaskCommandValidator : AbstractValidator<DeleteWorkTaskCommand>
    {
        public DeleteWorkTaskCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Task ID must be greater than 0.");
        }
    }
}

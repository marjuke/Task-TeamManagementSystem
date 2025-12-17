using Application.Features.WorkTasks.Queries;
using FluentValidation;

namespace Application.Validators
{
    public class GetFilteredTasksQueryValidator : AbstractValidator<GetFilteredTasksQuery>
    {
        public GetFilteredTasksQueryValidator()
        {
            RuleFor(x => x.StatusID)
                .GreaterThan(0).When(x => x.StatusID.HasValue)
                .WithMessage("Status ID must be greater than 0.");

            RuleFor(x => x.TeamId)
                .GreaterThan(0).When(x => x.TeamId.HasValue)
                .WithMessage("Team ID must be greater than 0.");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");

            RuleFor(x => x)
                .Custom((query, context) =>
                {
                    if (query.DueDateFrom.HasValue && query.DueDateTo.HasValue)
                    {
                        if (query.DueDateFrom.Value > query.DueDateTo.Value)
                        {
                            context.AddFailure("DueDateFrom", "Due date from cannot be greater than due date to.");
                        }
                    }
                });
        }
    }
}

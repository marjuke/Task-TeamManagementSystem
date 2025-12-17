using FluentValidation;
using Application.Validators;
using Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class FluentValidationExtension
    {
        public static IServiceCollection AddApplicationValidation(this IServiceCollection services)
        {
            // Register FluentValidation
            services.AddValidatorsFromAssemblyContaining<CreateWorkTaskCommandValidator>();

            // Register MediatR pipeline behavior for validation
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}

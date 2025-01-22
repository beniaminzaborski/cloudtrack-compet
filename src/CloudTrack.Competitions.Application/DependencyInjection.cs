using CloudTrack.Competitions.Application.Common;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CloudTrack.Competitions.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        var applicationAssembly = typeof(Application.Common.IUnitOfWork).Assembly;
        var domainAssembly = typeof(Domain.Common.IDomainEvent).Assembly;

        return service
            .AddMediatR(c =>
            { 
                c.RegisterServicesFromAssemblies(
                    applicationAssembly,
                    domainAssembly);

                c.AddOpenBehavior(typeof(ValidationBehavior<,>));
            })
            .AddApplicationServices()
            .AddValidatorsFromAssembly(applicationAssembly);
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .Scan(scan => scan.FromAssemblyOf<IApplicationService>()
            .AddClasses(classes => classes.AssignableTo<IApplicationService>())
            .AsMatchingInterface()
            .WithScopedLifetime());
    }
}

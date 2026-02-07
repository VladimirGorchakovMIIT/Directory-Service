using Directory_Service.Application.Abstraction;
using Directory_Service.Application.Department;
using Directory_Service.Application.Location;
using Directory_Service.Application.Location.Queries;
using Directory_Service.Application.Position;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Directory_Service.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes => classes.AssignableToAny(typeof(IQueriesHandler<,>),
                typeof(IQueriesHandler<>),
                typeof(IHandler<,>),
                typeof(IHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
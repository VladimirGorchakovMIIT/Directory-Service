using Directory_Service.Application.Location;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Directory_Service.Application.DependencyInjection;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjectionExtension).Assembly);
        services.AddScoped<CreateLocationHandler>();
        
        return services;
    }
}
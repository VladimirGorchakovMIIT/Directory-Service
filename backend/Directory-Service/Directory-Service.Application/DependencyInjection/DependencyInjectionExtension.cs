using Directory_Service.Application.Department;
using Directory_Service.Application.Location;
using Directory_Service.Application.Position;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Directory_Service.Application.DependencyInjection;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjectionExtension).Assembly);
        
        services.AddScoped<CreateLocationHandler>();
        services.AddScoped<CreateDepartmentHandler>();
        services.AddScoped<CreatePositionHandler>();
        
        return services;
    }
}
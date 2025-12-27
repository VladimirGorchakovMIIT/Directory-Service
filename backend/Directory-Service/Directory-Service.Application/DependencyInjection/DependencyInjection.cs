using Directory_Service.Application.Location;
using Microsoft.Extensions.DependencyInjection;

namespace Directory_Service.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddScoped<CreateLocationHandler>();
        
        return services;
    }
}
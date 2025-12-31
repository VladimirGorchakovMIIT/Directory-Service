using Directory_Service.Application.Location;
using Directory_Service.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Directory_Service.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
    {
        services.AddScoped<ILocationRepository, LocationRepository>();
        
        return services;
    }
}
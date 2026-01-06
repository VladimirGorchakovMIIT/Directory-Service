using Directory_Service.Application.Location;
using Directory_Service.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Directory_Service.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructurePostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ApplicationDbContext>( _ => new ApplicationDbContext(configuration));
        services.AddScoped<ILocationRepository, LocationRepository>();
        
        return services;
    }
}
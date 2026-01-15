using Directory_Service.Application.Database;
using Directory_Service.Application.Department;
using Directory_Service.Application.Location;
using Directory_Service.Application.Position;
using Directory_Service.Infrastructure.Database;
using Directory_Service.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Directory_Service.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructurePostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("ApplicationDbContext"));
            options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
        });

        services.AddScoped<ITransactionManager, TransactionManager>();

        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();

        return services;
    }
}
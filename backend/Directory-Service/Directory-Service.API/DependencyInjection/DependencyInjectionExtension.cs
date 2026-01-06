using Directory_Service.Application.DependencyInjection;
using Directory_Service.Infrastructure.DependencyInjection;
using Serilog;

namespace Directory_Service.Core.DependencyInjection;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        return AddSerilogLogging(services, configuration)
            .AddInfrastructurePostgres(configuration)
            .AddApplication();
    }

    private static IServiceCollection AddSerilogLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((sp, lc) => lc
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(sp)
            .Enrich.WithThreadId()
            .Enrich.FromLogContext());

        return services;
    }
}
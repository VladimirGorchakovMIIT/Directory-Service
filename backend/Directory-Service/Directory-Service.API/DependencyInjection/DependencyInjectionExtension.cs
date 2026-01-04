using Serilog;

namespace Directory_Service.Core.DependencyInjection;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddDirectoryService(this IServiceCollection services, IConfiguration configuration)
    {
        return AddSerilogLogging(services, configuration);
    }

    private static IServiceCollection AddSerilogLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((sp, lc) => lc
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(sp)
            .Enrich.WithThreadId()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ServiceName", "LessonService"));

        return services;
    }
}
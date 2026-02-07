using Directory_Service.Application.Department;
using Directory_Service.Domain.Location;
using Directory_Service.Domain.Location.ValueObjects;
using Directory_Service.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Directory_Service.IntegrationTests.Infrastructure;

public class DepartmentBaseTest : IClassFixture<DirectoryTestWebFactory>, IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;
    private IServiceProvider Services { get; set; }

    protected DepartmentBaseTest(DirectoryTestWebFactory factory)
    {
        _resetDatabase = factory.ResetDatabaseAsync;
        Services = factory.Services;
    }

    protected async Task<T> ExecuteHandlerChangeSubtree<T>(Func<MoveDepartmentHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        var sut = scope.ServiceProvider.GetRequiredService<MoveDepartmentHandler>();
        
        return await action(sut);
    }
    
    protected async Task<T> ExecuteHandlerCreate<T>(Func<CreateDepartmentHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        var sut = scope.ServiceProvider.GetRequiredService<CreateDepartmentHandler>();

        return await action(sut);
    }

    protected async Task<T> ExecuteHandlerUpdate<T>(Func<UpdateLocationsDepartmentHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        var sut = scope.ServiceProvider.GetRequiredService<UpdateLocationsDepartmentHandler>();
        
        return await action(sut);
    }

    protected async Task<T> ExecuteDb<T>(Func<ApplicationDbContext, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await action(dbContext);
    }

    protected async Task ExecuteDb(Func<ApplicationDbContext, Task> action)
    {
        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await action(dbContext);
    }

    protected async Task<LocationId> CreateLocations(string name, string street, string city)
    {
        return await ExecuteDb(async dbContext =>
        {
            var location = Location.Create(
                new LocationId(Guid.NewGuid()),
                name,
                new Address(street, city, 123, 33),
                new Timezone("Europe/Moscow"),
                false,
                []);

            dbContext.Locations.Add(location);
            await dbContext.SaveChangesAsync();

            return location.Id;
        });
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase();
}
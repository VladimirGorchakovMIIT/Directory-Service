using Directory_Service.Application.Department;
using Microsoft.Extensions.DependencyInjection;

namespace Directory_Service.IntegrationTests;

public class CreateDirectoryService : DirectoryTestWebFactory
{
    [Fact]
    public async Task CreateDepartment_with_valid_data_should_access()
    {
        await using var scope = Services.CreateAsyncScope();
        
        var sut = scope.ServiceProvider.GetRequiredService<CreateDepartmentHandler>();
        
        var cancellationToken = CancellationToken.None;

        var command = new CreateDepartmentCommand("Какое-то подразделение", "root-department", null, []);
        
        var result = sut.Handle(command, cancellationToken);
    }
}
using Directory_Service.Application.Department;
using Directory_Service.Domain.Department.ValueObjects;
using Directory_Service.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Directory_Service.IntegrationTests.Department;

public class CreateDepartmentTest(DirectoryTestWebFactory factory) : DepartmentBaseTest(factory)
{
    [Fact]
    public async Task CreateDepartment_with_valid_data_should_access()
    {
        //arrange
        var locationId = await CreateLocations("Название 1", "Улица 1", "Название города 1");
        var cancellationToken = CancellationToken.None;


        //act
        var result = await ExecuteHandlerCreate((sut) =>
        {
            var command = new CreateDepartmentCommand("Какое-то подразделение", "root-department", null, [locationId.Value]);
            return sut.Handle(command, cancellationToken);
        });
        
        //assert
        await ExecuteDb(async dbContext =>
        {
            var assertDepartment = await dbContext.Departments
                .FirstAsync(d => d.Id == new DepartmentId(result.Value), cancellationToken);

            Assert.NotNull(assertDepartment);
            Assert.Equal(assertDepartment.Id.Value, result.Value);

            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
        });
    }
}
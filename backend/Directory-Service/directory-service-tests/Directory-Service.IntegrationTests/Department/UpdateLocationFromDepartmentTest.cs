using Directory_Service.Application.Department;
using Directory_Service.Domain.Department;
using Directory_Service.Domain.Department.ValueObjects;
using Directory_Service.Domain.Location;
using Directory_Service.Domain.Location.ValueObjects;
using Directory_Service.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Directory_Service.IntegrationTests.Department;

public class UpdateLocationFromDepartmentTest(DirectoryTestWebFactory factory) : DepartmentBaseTest(factory)
{
    [Fact]
    public async Task UpdateLocation_from_department_should_access()
    {
        //arrange
        var locationIdForCreated = await CreateLocations("Название 1", "Улица 1", "Название города 1");
        var locationIdForUpdated = await CreateLocations("Название 2", "Улица 2", "Название города 2");

        var cancellationToken = CancellationToken.None;

        //act
        var result = await ExecuteHandlerCreate(sut =>
        {
            var command = new CreateDepartmentCommand("Какое-то подразделение", "root-department", null, [locationIdForCreated.Value]);
            return sut.Handle(command, cancellationToken);
        });

        var departmentId = result.Value;

        var beforeLocationUpdate = await GetLocationIds(departmentId, cancellationToken);

        await ExecuteHandlerUpdate(sut =>
        {
            var command = new UpdateLocationsDepartmentCommand(departmentId, [locationIdForUpdated.Value]);
            return sut.Handle(command, cancellationToken);
        });

        await ExecuteDb(async dbContext =>
        {
            var department = await dbContext.Departments.Where(d => d.Id == new DepartmentId(departmentId))
                .Include(d => d.DepartmentLocations)
                .FirstOrDefaultAsync(cancellationToken);

            var afterLocationUpdated = await GetLocationIds(departmentId, cancellationToken);

            Assert.NotNull(department);
            
            Assert.True(result.IsSuccess);
            Assert.False(EqualsCollectionsLocationIds(beforeLocationUpdate, afterLocationUpdated));
            
            Assert.NotEqual(Guid.Empty, departmentId);
            Assert.NotEqual(Guid.Empty, locationIdForCreated.Value);
        });
    }

    private async Task<IEnumerable<LocationId>> GetLocationIds(Guid departmentId, CancellationToken cancellationToken)
    {
        return await ExecuteDb(async dbContext =>
        {
            var department = await dbContext.Departments
                .Where(d => d.Id == new DepartmentId(departmentId))
                .Include(d => d.DepartmentLocations)
                .FirstAsync(cancellationToken);

            List<LocationId> locations = [];

            foreach (var item in department.DepartmentLocations)
                locations.Add(item.LocationId);

            return locations;
        });
    }

    private bool EqualsCollectionsLocationIds(IEnumerable<LocationId> before, IEnumerable<LocationId> after) => before.SequenceEqual(after);
}
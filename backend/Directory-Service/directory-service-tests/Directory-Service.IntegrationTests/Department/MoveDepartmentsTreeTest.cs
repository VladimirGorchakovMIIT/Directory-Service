using Directory_Service.Application.Department;
using Directory_Service.IntegrationTests.Infrastructure;

namespace Directory_Service.IntegrationTests.Department;

public class MoveDepartmentsTreeTest(DirectoryTestWebFactory factory) : DepartmentBaseTest(factory)
{
    [Fact]
    public async Task ChangingDepartmentChild_tree_and_checking_for_correctness()
    {
        //arrange
        CancellationToken cancellationToken = CancellationToken.None;

        var locationId = await CreateLocations("Name 1", "Street 1", "City 1");

        var root = await CreateDepartment(
            new CreateDepartmentCommand("Главный офис",
                "root",
                null,
                [locationId.Value]),
            cancellationToken);

        var manager = await CreateDepartment(
            new CreateDepartmentCommand("Офис с менеджерами",
                "manager",
                root,
                [locationId.Value]),
            cancellationToken);

        var managerFirstSupport = await CreateDepartment(
            new CreateDepartmentCommand("Первый помощник менеджера",
                "manager-first",
                manager,
                [locationId.Value]),
            cancellationToken);

        var managerSecondSupport = await CreateDepartment(
            new CreateDepartmentCommand("Второй помощник менеджера",
                "manager-second",
                manager,
                [locationId.Value]),
            cancellationToken);

        var engine = await CreateDepartment(
            new CreateDepartmentCommand("Офис с инженерами",
                "engine",
                root,
                [locationId.Value]),
            cancellationToken);

        var engineFirstSupport = await CreateDepartment(
            new CreateDepartmentCommand("Первый помощник инженера",
                "engine-first",
                engine,
                [locationId.Value]),
            cancellationToken);

        var engineSecondSupport = await CreateDepartment(
            new CreateDepartmentCommand("Второй помощник инженера",
                "engine-second",
                engine,
                [locationId.Value]),
            cancellationToken);

        //act
        var result = await ExecuteHandlerChangeSubtree(sut =>
        {
            var command = new ChangeDepartmentCommand(engine, manager);
            return sut.Handle(command, cancellationToken);
        });
        
        //assert
        Assert.True(result.IsSuccess);
        Assert.Equal(engine, result.Value);
    }

    private async Task<Guid> CreateDepartment(CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        var departmentId = await ExecuteHandlerCreate(sut => sut.Handle(command, cancellationToken));
        return departmentId.Value;
    }
}
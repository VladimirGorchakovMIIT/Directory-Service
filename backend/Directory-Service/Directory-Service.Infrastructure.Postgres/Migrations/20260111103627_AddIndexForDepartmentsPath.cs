using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Directory_Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexForDepartmentsPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "idx_department_path",
                table: "department",
                column: "path")
                .Annotation("Npgsql:IndexMethod", "gist");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_department_path",
                table: "department");
        }
    }
}

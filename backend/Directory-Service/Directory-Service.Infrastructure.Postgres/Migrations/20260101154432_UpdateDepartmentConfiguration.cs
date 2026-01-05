using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Directory_Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDepartmentConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_department_department_parent_id",
                table: "department");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentDepartmentId",
                table: "department",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_department_ParentDepartmentId",
                table: "department",
                column: "ParentDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_department_department_ParentDepartmentId",
                table: "department",
                column: "ParentDepartmentId",
                principalTable: "department",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_department_department_parent_id",
                table: "department",
                column: "parent_id",
                principalTable: "department",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_department_department_ParentDepartmentId",
                table: "department");

            migrationBuilder.DropForeignKey(
                name: "FK_department_department_parent_id",
                table: "department");

            migrationBuilder.DropIndex(
                name: "IX_department_ParentDepartmentId",
                table: "department");

            migrationBuilder.DropColumn(
                name: "ParentDepartmentId",
                table: "department");

            migrationBuilder.AddForeignKey(
                name: "FK_department_department_parent_id",
                table: "department",
                column: "parent_id",
                principalTable: "department",
                principalColumn: "id");
        }
    }
}

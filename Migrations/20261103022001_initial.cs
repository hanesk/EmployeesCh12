using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeesCh12.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentLocations_Departments_LocationID",
                table: "DepartmentLocations");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentLocations_Departments_DepartmentID",
                table: "DepartmentLocations",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentLocations_Departments_DepartmentID",
                table: "DepartmentLocations");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentLocations_Departments_LocationID",
                table: "DepartmentLocations",
                column: "LocationID",
                principalTable: "Departments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

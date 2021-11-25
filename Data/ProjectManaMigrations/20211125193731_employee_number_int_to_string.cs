using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProject.Data.ProjectManaMigrations
{
    public partial class employee_number_int_to_string : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Manager",
                table: "MemberProject");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeNumber",
                table: "Member",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 6);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Manager",
                table: "MemberProject",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeNumber",
                table: "Member",
                type: "int",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);
        }
    }
}

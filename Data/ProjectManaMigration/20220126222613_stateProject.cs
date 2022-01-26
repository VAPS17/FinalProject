using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProject.Migrations
{
    public partial class stateProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "Project",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Project_StateId",
                table: "Project",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_State_StateId",
                table: "Project",
                column: "StateId",
                principalTable: "State",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_State_StateId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_StateId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Project");
        }
    }
}

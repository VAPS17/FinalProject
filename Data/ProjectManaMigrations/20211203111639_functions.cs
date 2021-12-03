using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProject.Data.ProjectManaMigrations
{
    public partial class functions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FunctionId",
                table: "Member",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Function",
                columns: table => new
                {
                    FunctionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Function", x => x.FunctionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Member_FunctionId",
                table: "Member",
                column: "FunctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Function_FunctionId",
                table: "Member",
                column: "FunctionId",
                principalTable: "Function",
                principalColumn: "FunctionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_Function_FunctionId",
                table: "Member");

            migrationBuilder.DropTable(
                name: "Function");

            migrationBuilder.DropIndex(
                name: "IX_Member_FunctionId",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "FunctionId",
                table: "Member");
        }
    }
}

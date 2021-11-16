using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProject.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "P_Task",
                columns: table => new
                {
                    P_TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    P_TaskName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Comentary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    P_TaskState = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_P_Task", x => x.P_TaskId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "P_Task");
        }
    }
}

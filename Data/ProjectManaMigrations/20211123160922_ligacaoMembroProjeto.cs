using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProject.Data.ProjectManaMigrations
{
    public partial class ligacaoMembroProjeto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_Project_ProjectId",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_P_Task_Project_ProjectId",
                table: "P_Task");

            migrationBuilder.DropIndex(
                name: "IX_Member_ProjectId",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "ProjectState",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "MeetingTopic",
                table: "Meeting");

            migrationBuilder.RenameColumn(
                name: "ProjectName",
                table: "Project",
                newName: "ProjectCreator");

            migrationBuilder.RenameColumn(
                name: "Annotations",
                table: "Project",
                newName: "NumberEmployees");

            migrationBuilder.RenameColumn(
                name: "MeetingDateandTime",
                table: "Meeting",
                newName: "DateandTime");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Project",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DecisiveDeliveryDate",
                table: "Project",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Project",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Meeting",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Meeting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "Meeting",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MemberProject",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Manager = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberProject", x => new { x.MemberId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_MemberProject_Member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberProject_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meeting_ProjectId",
                table: "Meeting",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberProject_ProjectId",
                table: "MemberProject",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meeting_Project_ProjectId",
                table: "Meeting",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_P_Task_Project_ProjectId",
                table: "P_Task",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meeting_Project_ProjectId",
                table: "Meeting");

            migrationBuilder.DropForeignKey(
                name: "FK_P_Task_Project_ProjectId",
                table: "P_Task");

            migrationBuilder.DropTable(
                name: "MemberProject");

            migrationBuilder.DropIndex(
                name: "IX_Meeting_ProjectId",
                table: "Meeting");

            migrationBuilder.DropColumn(
                name: "DecisiveDeliveryDate",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Meeting");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Meeting");

            migrationBuilder.DropColumn(
                name: "Topic",
                table: "Meeting");

            migrationBuilder.RenameColumn(
                name: "ProjectCreator",
                table: "Project",
                newName: "ProjectName");

            migrationBuilder.RenameColumn(
                name: "NumberEmployees",
                table: "Project",
                newName: "Annotations");

            migrationBuilder.RenameColumn(
                name: "DateandTime",
                table: "Meeting",
                newName: "MeetingDateandTime");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ProjectState",
                table: "Project",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Manager",
                table: "Member",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Member",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MeetingTopic",
                table: "Meeting",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Member_ProjectId",
                table: "Member",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Project_ProjectId",
                table: "Member",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_P_Task_Project_ProjectId",
                table: "P_Task",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

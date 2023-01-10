using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holonet.Jedi.Academy.App.Migrations.Academy
{
    public partial class StudentExpFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AddedToStudent",
                schema: "App",
                table: "QuestXP",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AddedToStudent",
                schema: "App",
                table: "KnowledgeXP",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedToStudent",
                schema: "App",
                table: "QuestXP");

            migrationBuilder.DropColumn(
                name: "AddedToStudent",
                schema: "App",
                table: "KnowledgeXP");
        }
    }
}

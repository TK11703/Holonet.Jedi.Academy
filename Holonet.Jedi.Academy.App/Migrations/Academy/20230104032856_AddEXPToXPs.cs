using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holonet.Jedi.Academy.App.Migrations.Academy
{
    public partial class AddEXPToXPs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExperienceToGain",
                schema: "App",
                table: "QuestXP",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExperienceToGain",
                schema: "App",
                table: "KnowledgeXP",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExperienceToGain",
                schema: "App",
                table: "QuestXP");

            migrationBuilder.DropColumn(
                name: "ExperienceToGain",
                schema: "App",
                table: "KnowledgeXP");
        }
    }
}

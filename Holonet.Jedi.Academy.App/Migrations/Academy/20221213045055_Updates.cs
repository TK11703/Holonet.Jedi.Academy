using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holonet.Jedi.Academy.App.Migrations.Academy
{
    public partial class Updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                schema: "App",
                table: "Quests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archived",
                schema: "App",
                table: "Quests");
        }
    }
}

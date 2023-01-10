using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holonet.Jedi.Academy.App.Migrations.Academy
{
    public partial class ReferenceArchiveable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                schema: "App",
                table: "TerminationReasons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                schema: "App",
                table: "Species",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                schema: "App",
                table: "Ranks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                schema: "App",
                table: "Planets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                schema: "App",
                table: "ForcePowers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archived",
                schema: "App",
                table: "TerminationReasons");

            migrationBuilder.DropColumn(
                name: "Archived",
                schema: "App",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "Archived",
                schema: "App",
                table: "Ranks");

            migrationBuilder.DropColumn(
                name: "Archived",
                schema: "App",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "Archived",
                schema: "App",
                table: "ForcePowers");
        }
    }
}

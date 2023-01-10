using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holonet.Jedi.Academy.App.Migrations.Academy
{
    public partial class AddRank2FP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinimumRankId",
                schema: "App",
                table: "ForcePowers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ForcePowers_MinimumRankId",
                schema: "App",
                table: "ForcePowers",
                column: "MinimumRankId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForcePowers_Ranks_MinimumRankId",
                schema: "App",
                table: "ForcePowers",
                column: "MinimumRankId",
                principalSchema: "App",
                principalTable: "Ranks",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForcePowers_Ranks_MinimumRankId",
                schema: "App",
                table: "ForcePowers");

            migrationBuilder.DropIndex(
                name: "IX_ForcePowers_MinimumRankId",
                schema: "App",
                table: "ForcePowers");

            migrationBuilder.DropColumn(
                name: "MinimumRankId",
                schema: "App",
                table: "ForcePowers");
        }
    }
}

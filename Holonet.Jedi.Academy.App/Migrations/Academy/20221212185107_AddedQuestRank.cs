using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holonet.Jedi.Academy.App.Migrations.Academy
{
    public partial class AddedQuestRank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RankId",
                schema: "App",
                table: "Quests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Quests_RankId",
                schema: "App",
                table: "Quests",
                column: "RankId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quests_Ranks_RankId",
                schema: "App",
                table: "Quests",
                column: "RankId",
                principalSchema: "App",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quests_Ranks_RankId",
                schema: "App",
                table: "Quests");

            migrationBuilder.DropIndex(
                name: "IX_Quests_RankId",
                schema: "App",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "RankId",
                schema: "App",
                table: "Quests");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holonet.Jedi.Academy.App.Migrations.Academy
{
    public partial class AlterRewardPt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RewardPoints_KnowledgeOpportunities_KnowledgeId",
                schema: "App",
                table: "RewardPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_RewardPoints_Quests_QuestId",
                schema: "App",
                table: "RewardPoints");

            migrationBuilder.DropIndex(
                name: "IX_RewardPoints_KnowledgeId",
                schema: "App",
                table: "RewardPoints");

            migrationBuilder.DropColumn(
                name: "KnowledgeId",
                schema: "App",
                table: "RewardPoints");

            migrationBuilder.RenameColumn(
                name: "QuestId",
                schema: "App",
                table: "RewardPoints",
                newName: "RankId");

            migrationBuilder.RenameIndex(
                name: "IX_RewardPoints_QuestId",
                schema: "App",
                table: "RewardPoints",
                newName: "IX_RewardPoints_RankId");

            migrationBuilder.AddForeignKey(
                name: "FK_RewardPoints_Ranks_RankId",
                schema: "App",
                table: "RewardPoints",
                column: "RankId",
                principalSchema: "App",
                principalTable: "Ranks",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RewardPoints_Ranks_RankId",
                schema: "App",
                table: "RewardPoints");

            migrationBuilder.RenameColumn(
                name: "RankId",
                schema: "App",
                table: "RewardPoints",
                newName: "QuestId");

            migrationBuilder.RenameIndex(
                name: "IX_RewardPoints_RankId",
                schema: "App",
                table: "RewardPoints",
                newName: "IX_RewardPoints_QuestId");

            migrationBuilder.AddColumn<int>(
                name: "KnowledgeId",
                schema: "App",
                table: "RewardPoints",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RewardPoints_KnowledgeId",
                schema: "App",
                table: "RewardPoints",
                column: "KnowledgeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RewardPoints_KnowledgeOpportunities_KnowledgeId",
                schema: "App",
                table: "RewardPoints",
                column: "KnowledgeId",
                principalSchema: "App",
                principalTable: "KnowledgeOpportunities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RewardPoints_Quests_QuestId",
                schema: "App",
                table: "RewardPoints",
                column: "QuestId",
                principalSchema: "App",
                principalTable: "Quests",
                principalColumn: "Id");
        }
    }
}

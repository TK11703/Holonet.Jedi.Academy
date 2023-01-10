using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holonet.Jedi.Academy.App.Migrations.Academy
{
    public partial class NotifRewardPt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RewardPoints",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    KnowledgeId = table.Column<int>(type: "int", nullable: true),
                    QuestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RewardPoints_KnowledgeOpportunities_KnowledgeId",
                        column: x => x.KnowledgeId,
                        principalSchema: "App",
                        principalTable: "KnowledgeOpportunities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RewardPoints_Quests_QuestId",
                        column: x => x.QuestId,
                        principalSchema: "App",
                        principalTable: "Quests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RewardPoints_Students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "App",
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RewardPoints_KnowledgeId",
                schema: "App",
                table: "RewardPoints",
                column: "KnowledgeId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardPoints_QuestId",
                schema: "App",
                table: "RewardPoints",
                column: "QuestId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardPoints_StudentId",
                schema: "App",
                table: "RewardPoints",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RewardPoints",
                schema: "App");
        }
    }
}

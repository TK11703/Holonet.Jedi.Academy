using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holonet.Jedi.Academy.App.Migrations.Academy
{
    public partial class QuestUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "App",
                table: "Quests",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "App",
                table: "KnowledgeOpportunities",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QuestDestinations",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanetId = table.Column<int>(type: "int", nullable: false),
                    QuestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestDestinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestDestinations_Planets_PlanetId",
                        column: x => x.PlanetId,
                        principalSchema: "App",
                        principalTable: "Planets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestDestinations_Quests_QuestId",
                        column: x => x.QuestId,
                        principalSchema: "App",
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestDestinations_PlanetId",
                schema: "App",
                table: "QuestDestinations",
                column: "PlanetId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestDestinations_QuestId",
                schema: "App",
                table: "QuestDestinations",
                column: "QuestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestDestinations",
                schema: "App");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "App",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "App",
                table: "KnowledgeOpportunities");
        }
    }
}

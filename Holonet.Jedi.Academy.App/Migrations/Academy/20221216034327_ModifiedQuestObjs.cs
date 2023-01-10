using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holonet.Jedi.Academy.App.Migrations.Academy
{
    public partial class ModifiedQuestObjs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestDestinations",
                schema: "App");

            migrationBuilder.CreateTable(
                name: "Objectives",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Archived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objectives", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompletedObjectives",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestXPId = table.Column<int>(type: "int", nullable: false),
                    ObjectiveId = table.Column<int>(type: "int", nullable: false),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedObjectives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedObjectives_Objectives_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalSchema: "App",
                        principalTable: "Objectives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompletedObjectives_QuestXP_QuestXPId",
                        column: x => x.QuestXPId,
                        principalSchema: "App",
                        principalTable: "QuestXP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObjectiveDestinations",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanetId = table.Column<int>(type: "int", nullable: false),
                    ObjectiveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectiveDestinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectiveDestinations_Objectives_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalSchema: "App",
                        principalTable: "Objectives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObjectiveDestinations_Planets_PlanetId",
                        column: x => x.PlanetId,
                        principalSchema: "App",
                        principalTable: "Planets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestObjectives",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectiveId = table.Column<int>(type: "int", nullable: false),
                    QuestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestObjectives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestObjectives_Objectives_ObjectiveId",
                        column: x => x.ObjectiveId,
                        principalSchema: "App",
                        principalTable: "Objectives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestObjectives_Quests_QuestId",
                        column: x => x.QuestId,
                        principalSchema: "App",
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedObjectives_ObjectiveId",
                schema: "App",
                table: "CompletedObjectives",
                column: "ObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedObjectives_QuestXPId",
                schema: "App",
                table: "CompletedObjectives",
                column: "QuestXPId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveDestinations_ObjectiveId",
                schema: "App",
                table: "ObjectiveDestinations",
                column: "ObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectiveDestinations_PlanetId",
                schema: "App",
                table: "ObjectiveDestinations",
                column: "PlanetId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestObjectives_ObjectiveId",
                schema: "App",
                table: "QuestObjectives",
                column: "ObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestObjectives_QuestId",
                schema: "App",
                table: "QuestObjectives",
                column: "QuestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedObjectives",
                schema: "App");

            migrationBuilder.DropTable(
                name: "ObjectiveDestinations",
                schema: "App");

            migrationBuilder.DropTable(
                name: "QuestObjectives",
                schema: "App");

            migrationBuilder.DropTable(
                name: "Objectives",
                schema: "App");

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
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holonet.Jedi.Academy.App.Migrations.Academy
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "App");

            migrationBuilder.CreateTable(
                name: "ForcePowers",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForcePowers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeOpportunities",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ExperienceToGain = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeOpportunities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Planets",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quests",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ExperienceToGain = table.Column<int>(type: "int", nullable: false),
                    MinimumRankLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ranks",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RankLevel = table.Column<int>(type: "int", nullable: false),
                    Minimum = table.Column<int>(type: "int", nullable: false),
                    Maximum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Species",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TerminationReasons",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminationReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SpeciesId = table.Column<int>(type: "int", nullable: false),
                    PlanetId = table.Column<int>(type: "int", nullable: false),
                    InitiatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeftOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReasonForTerminationId = table.Column<int>(type: "int", nullable: false),
                    RankId = table.Column<int>(type: "int", nullable: false),
                    Experience = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Planets_PlanetId",
                        column: x => x.PlanetId,
                        principalSchema: "App",
                        principalTable: "Planets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Ranks_RankId",
                        column: x => x.RankId,
                        principalSchema: "App",
                        principalTable: "Ranks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalSchema: "App",
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_TerminationReasons_ReasonForTerminationId",
                        column: x => x.ReasonForTerminationId,
                        principalSchema: "App",
                        principalTable: "TerminationReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForcePowerXP",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForcePowerId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    GainedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForcePowerXP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForcePowerXP_ForcePowers_ForcePowerId",
                        column: x => x.ForcePowerId,
                        principalSchema: "App",
                        principalTable: "ForcePowers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForcePowerXP_Students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "App",
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeXP",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KnowledgeId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    StartedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeXP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KnowledgeXP_KnowledgeOpportunities_KnowledgeId",
                        column: x => x.KnowledgeId,
                        principalSchema: "App",
                        principalTable: "KnowledgeOpportunities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KnowledgeXP_Students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "App",
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestXP",
                schema: "App",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    StartedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestXP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestXP_Quests_QuestId",
                        column: x => x.QuestId,
                        principalSchema: "App",
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestXP_Students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "App",
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForcePowerXP_ForcePowerId",
                schema: "App",
                table: "ForcePowerXP",
                column: "ForcePowerId");

            migrationBuilder.CreateIndex(
                name: "IX_ForcePowerXP_StudentId",
                schema: "App",
                table: "ForcePowerXP",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeXP_KnowledgeId",
                schema: "App",
                table: "KnowledgeXP",
                column: "KnowledgeId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeXP_StudentId",
                schema: "App",
                table: "KnowledgeXP",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestXP_QuestId",
                schema: "App",
                table: "QuestXP",
                column: "QuestId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestXP_StudentId",
                schema: "App",
                table: "QuestXP",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_PlanetId",
                schema: "App",
                table: "Students",
                column: "PlanetId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_RankId",
                schema: "App",
                table: "Students",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ReasonForTerminationId",
                schema: "App",
                table: "Students",
                column: "ReasonForTerminationId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SpeciesId",
                schema: "App",
                table: "Students",
                column: "SpeciesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForcePowerXP",
                schema: "App");

            migrationBuilder.DropTable(
                name: "KnowledgeXP",
                schema: "App");

            migrationBuilder.DropTable(
                name: "QuestXP",
                schema: "App");

            migrationBuilder.DropTable(
                name: "ForcePowers",
                schema: "App");

            migrationBuilder.DropTable(
                name: "KnowledgeOpportunities",
                schema: "App");

            migrationBuilder.DropTable(
                name: "Quests",
                schema: "App");

            migrationBuilder.DropTable(
                name: "Students",
                schema: "App");

            migrationBuilder.DropTable(
                name: "Planets",
                schema: "App");

            migrationBuilder.DropTable(
                name: "Ranks",
                schema: "App");

            migrationBuilder.DropTable(
                name: "Species",
                schema: "App");

            migrationBuilder.DropTable(
                name: "TerminationReasons",
                schema: "App");
        }
    }
}

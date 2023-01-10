using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holonet.Jedi.Academy.App.Migrations.Academy
{
    public partial class AddKnowledgeUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                schema: "App",
                table: "KnowledgeOpportunities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "App",
                table: "KnowledgeOpportunities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archived",
                schema: "App",
                table: "KnowledgeOpportunities");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "App",
                table: "KnowledgeOpportunities");
        }
    }
}

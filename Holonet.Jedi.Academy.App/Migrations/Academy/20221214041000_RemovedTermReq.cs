using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holonet.Jedi.Academy.App.Migrations.Academy
{
    public partial class RemovedTermReq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_TerminationReasons_ReasonForTerminationId",
                schema: "App",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "ReasonForTerminationId",
                schema: "App",
                table: "Students",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_TerminationReasons_ReasonForTerminationId",
                schema: "App",
                table: "Students",
                column: "ReasonForTerminationId",
                principalSchema: "App",
                principalTable: "TerminationReasons",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_TerminationReasons_ReasonForTerminationId",
                schema: "App",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "ReasonForTerminationId",
                schema: "App",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_TerminationReasons_ReasonForTerminationId",
                schema: "App",
                table: "Students",
                column: "ReasonForTerminationId",
                principalSchema: "App",
                principalTable: "TerminationReasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

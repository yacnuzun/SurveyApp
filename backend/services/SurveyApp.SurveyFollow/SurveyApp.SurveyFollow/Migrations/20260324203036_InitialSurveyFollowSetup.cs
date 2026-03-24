using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyApp.SurveyFollow.Migrations
{
    /// <inheritdoc />
    public partial class InitialSurveyFollowSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "public",
                table: "Participations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "public",
                table: "Participations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "public",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "public",
                table: "Answers");

            migrationBuilder.RenameTable(
                name: "Participations",
                schema: "public",
                newName: "Participations");

            migrationBuilder.RenameTable(
                name: "Answers",
                schema: "public",
                newName: "Answers");

            migrationBuilder.AlterColumn<string>(
                name: "TextAnswer",
                table: "Answers",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participations_SurveyId_UserId",
                table: "Participations",
                columns: new[] { "SurveyId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Participations_SurveyId_UserId",
                table: "Participations");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "Participations",
                newName: "Participations",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Answers",
                newName: "Answers",
                newSchema: "public");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "public",
                table: "Participations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "public",
                table: "Participations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "TextAnswer",
                schema: "public",
                table: "Answers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "public",
                table: "Answers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "public",
                table: "Answers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyApp.ReportingService.Migrations
{
    /// <inheritdoc />
    public partial class InitialReportingSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Statistics",
                schema: "public",
                table: "Statistics");

            migrationBuilder.RenameTable(
                name: "Statistics",
                schema: "public",
                newName: "SurveyStatistics");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionText",
                table: "SurveyStatistics",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "OptionText",
                table: "SurveyStatistics",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyStatistics",
                table: "SurveyStatistics",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyStatistics_SurveyId",
                table: "SurveyStatistics",
                column: "SurveyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyStatistics",
                table: "SurveyStatistics");

            migrationBuilder.DropIndex(
                name: "IX_SurveyStatistics_SurveyId",
                table: "SurveyStatistics");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "SurveyStatistics",
                newName: "Statistics",
                newSchema: "public");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionText",
                schema: "public",
                table: "Statistics",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "OptionText",
                schema: "public",
                table: "Statistics",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Statistics",
                schema: "public",
                table: "Statistics",
                column: "Id");
        }
    }
}

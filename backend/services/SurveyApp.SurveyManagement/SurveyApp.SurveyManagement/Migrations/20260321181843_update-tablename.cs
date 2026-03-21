using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyApp.SurveyManagement.Migrations
{
    /// <inheritdoc />
    public partial class updatetablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationClaims_UserOperationClaims_SurveyId",
                schema: "public",
                table: "OperationClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_OperationClaims_QuestionId",
                schema: "public",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "public",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOperationClaims",
                schema: "public",
                table: "UserOperationClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OperationClaims",
                schema: "public",
                table: "OperationClaims");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "public",
                newName: "Options",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "UserOperationClaims",
                schema: "public",
                newName: "Surveys",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "OperationClaims",
                schema: "public",
                newName: "Questions",
                newSchema: "public");

            migrationBuilder.RenameIndex(
                name: "IX_Users_QuestionId",
                schema: "public",
                table: "Options",
                newName: "IX_Options_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_OperationClaims_SurveyId",
                schema: "public",
                table: "Questions",
                newName: "IX_Questions_SurveyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Options",
                schema: "public",
                table: "Options",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Surveys",
                schema: "public",
                table: "Surveys",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                schema: "public",
                table: "Questions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Questions_QuestionId",
                schema: "public",
                table: "Options",
                column: "QuestionId",
                principalSchema: "public",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Surveys_SurveyId",
                schema: "public",
                table: "Questions",
                column: "SurveyId",
                principalSchema: "public",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Questions_QuestionId",
                schema: "public",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Surveys_SurveyId",
                schema: "public",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Surveys",
                schema: "public",
                table: "Surveys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                schema: "public",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Options",
                schema: "public",
                table: "Options");

            migrationBuilder.RenameTable(
                name: "Surveys",
                schema: "public",
                newName: "UserOperationClaims",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Questions",
                schema: "public",
                newName: "OperationClaims",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "Options",
                schema: "public",
                newName: "Users",
                newSchema: "public");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_SurveyId",
                schema: "public",
                table: "OperationClaims",
                newName: "IX_OperationClaims_SurveyId");

            migrationBuilder.RenameIndex(
                name: "IX_Options_QuestionId",
                schema: "public",
                table: "Users",
                newName: "IX_Users_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOperationClaims",
                schema: "public",
                table: "UserOperationClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OperationClaims",
                schema: "public",
                table: "OperationClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "public",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationClaims_UserOperationClaims_SurveyId",
                schema: "public",
                table: "OperationClaims",
                column: "SurveyId",
                principalSchema: "public",
                principalTable: "UserOperationClaims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_OperationClaims_QuestionId",
                schema: "public",
                table: "Users",
                column: "QuestionId",
                principalSchema: "public",
                principalTable: "OperationClaims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

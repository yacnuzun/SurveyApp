using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyApp.SurveyManagement.Migrations
{
    /// <inheritdoc />
    public partial class updatecustomeridtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE public.\"UserOperationClaims\" ALTER COLUMN \"CreatedBy\" TYPE integer USING (0);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                schema: "public",
                table: "UserOperationClaims",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}

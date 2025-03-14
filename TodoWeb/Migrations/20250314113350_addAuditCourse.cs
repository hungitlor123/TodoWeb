using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoWeb.Migrations
{
    /// <inheritdoc />
    public partial class addAuditCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Course",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Course",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Course");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IKEA.DAL.Presistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "Departments");

            migrationBuilder.AddColumn<int>(
                name: "LastModificationBy",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationOn",
                table: "Departments",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "GETUTCDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModificationOn",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "LastModificationBy",
                table: "Departments");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "Departments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                table: "Departments",
                type: "int",
                nullable: false,
                computedColumnSql: "GETDATE()");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace southernTravel.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "products",
                newName: "start_date");

            migrationBuilder.RenameColumn(
                name: "imageUr1",
                table: "products",
                newName: "image_ur1");

            migrationBuilder.RenameColumn(
                name: "goStartDate",
                table: "products",
                newName: "go_start_date");

            migrationBuilder.RenameColumn(
                name: "goEndDate",
                table: "products",
                newName: "go_end_date");

            migrationBuilder.RenameColumn(
                name: "endDate",
                table: "products",
                newName: "end_date");

            migrationBuilder.RenameColumn(
                name: "creationDate",
                table: "products",
                newName: "updated_at");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "products",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "products",
                newName: "startDate");

            migrationBuilder.RenameColumn(
                name: "image_ur1",
                table: "products",
                newName: "imageUr1");

            migrationBuilder.RenameColumn(
                name: "go_start_date",
                table: "products",
                newName: "goStartDate");

            migrationBuilder.RenameColumn(
                name: "go_end_date",
                table: "products",
                newName: "goEndDate");

            migrationBuilder.RenameColumn(
                name: "end_date",
                table: "products",
                newName: "endDate");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "products",
                newName: "creationDate");
        }
    }
}

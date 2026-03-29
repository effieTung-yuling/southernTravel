using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace southernTravel.Migrations
{
    /// <inheritdoc />
    public partial class AddSortOrderToProductImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "productimages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "productimages");
        }
    }
}

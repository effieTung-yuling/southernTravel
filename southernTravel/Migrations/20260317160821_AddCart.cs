using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace southernTravel.Migrations
{
    /// <inheritdoc />
    public partial class AddCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itinerary_products_ProductId",
                table: "Itinerary");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Itinerary",
                table: "Itinerary");

            migrationBuilder.RenameTable(
                name: "Itinerary",
                newName: "Itineraries");

            migrationBuilder.RenameIndex(
                name: "IX_Itinerary_ProductId",
                table: "Itineraries",
                newName: "IX_Itineraries_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Itineraries",
                table: "Itineraries",
                column: "ItineraryId");

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    FinalTotal = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CartId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Qty = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<int>(type: "INTEGER", nullable: false),
                    Total = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Itineraries_products_ProductId",
                table: "Itineraries",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itineraries_products_ProductId",
                table: "Itineraries");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Itineraries",
                table: "Itineraries");

            migrationBuilder.RenameTable(
                name: "Itineraries",
                newName: "Itinerary");

            migrationBuilder.RenameIndex(
                name: "IX_Itineraries_ProductId",
                table: "Itinerary",
                newName: "IX_Itinerary_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Itinerary",
                table: "Itinerary",
                column: "ItineraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Itinerary_products_ProductId",
                table: "Itinerary",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "product_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

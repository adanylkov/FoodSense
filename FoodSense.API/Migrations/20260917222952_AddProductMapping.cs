using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodSense.API.Migrations
{
    /// <inheritdoc />
    public partial class AddProductMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductMappings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductMappings_ProductId",
                table: "ProductMappings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMappings_ProductName",
                table: "ProductMappings",
                column: "ProductName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductMappings");
        }
    }
}

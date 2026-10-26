using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodSense.API.Migrations
{
    /// <inheritdoc />
    public partial class FoodEntryTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodLogEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    MealType = table.Column<int>(type: "int", nullable: false),
                    ConsumedQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Calories = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    Protein = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    Fat = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    Carbs = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodLogEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodLogEntries_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserNutritionalGoals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetCalories = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TargetProtein = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TargetFat = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TargetCarbs = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNutritionalGoals", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodLogEntries_ProductId",
                table: "FoodLogEntries",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodLogEntries_UserId_Date",
                table: "FoodLogEntries",
                columns: new[] { "UserId", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodLogEntries");

            migrationBuilder.DropTable(
                name: "UserNutritionalGoals");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodSense.API.Migrations
{
    /// <inheritdoc />
    public partial class FoodLogRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Nutrients_EnergyKcal",
                table: "Products",
                newName: "Nutrients_Kcal");

            migrationBuilder.RenameColumn(
                name: "Protein",
                table: "FoodLogEntries",
                newName: "Nutritions_Protein");

            migrationBuilder.RenameColumn(
                name: "Fat",
                table: "FoodLogEntries",
                newName: "Nutritions_Fat");

            migrationBuilder.RenameColumn(
                name: "Carbs",
                table: "FoodLogEntries",
                newName: "Nutritions_Carbs");

            migrationBuilder.RenameColumn(
                name: "Calories",
                table: "FoodLogEntries",
                newName: "Nutritions_Calories");

            migrationBuilder.AlterColumn<double>(
                name: "Nutritions_Protein",
                table: "FoodLogEntries",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "Nutritions_Fat",
                table: "FoodLogEntries",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "Nutritions_Carbs",
                table: "FoodLogEntries",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "Nutritions_Calories",
                table: "FoodLogEntries",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nutrients_Kcal",
                table: "Products",
                newName: "Nutrients_EnergyKcal");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "Nutritions_Protein",
                table: "FoodLogEntries",
                newName: "Protein");

            migrationBuilder.RenameColumn(
                name: "Nutritions_Fat",
                table: "FoodLogEntries",
                newName: "Fat");

            migrationBuilder.RenameColumn(
                name: "Nutritions_Carbs",
                table: "FoodLogEntries",
                newName: "Carbs");

            migrationBuilder.RenameColumn(
                name: "Nutritions_Calories",
                table: "FoodLogEntries",
                newName: "Calories");

            migrationBuilder.AlterColumn<double>(
                name: "Protein",
                table: "FoodLogEntries",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Fat",
                table: "FoodLogEntries",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Carbs",
                table: "FoodLogEntries",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Calories",
                table: "FoodLogEntries",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}

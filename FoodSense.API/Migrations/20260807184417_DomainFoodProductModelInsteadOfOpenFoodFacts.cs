using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodSense.API.Migrations
{
    /// <inheritdoc />
    public partial class DomainFoodProductModelInsteadOfOpenFoodFacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_LanguagesCodes_LanguagesCodesEn_LanguagesCodesFr_LanguagesCodesPl",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_NutrientLevels_NutrientLevelsSalt_NutrientLevelsSugars_NutrientLevelsSaturatedFat_NutrientLevelsFat",
                table: "Products");

            migrationBuilder.DropTable(
                name: "LanguagesCodes");

            migrationBuilder.DropTable(
                name: "NutrientLevels");

            migrationBuilder.DropIndex(
                name: "IX_Products_LanguagesCodesEn_LanguagesCodesFr_LanguagesCodesPl",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_NutrientLevelsSalt_NutrientLevelsSugars_NutrientLevelsSaturatedFat_NutrientLevelsFat",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AdditivesDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AdditivesN",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AdditivesOldN",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AdditivesOldTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AdditivesOriginalTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AdditivesPrevOriginalTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AdditivesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AllergensFromIngredients",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AllergensFromUser",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AllergensHierarchy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AllergensLc",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AllergensTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AminoAcidsPrevTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AminoAcidsTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Brands",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BrandsDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BrandsTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CarbonFootprintFromKnownIngredientsDebug",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CarbonFootprintPercentOfKnownIngredients",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoriesHierarchy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoriesLc",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoriesPropertiesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoriesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CheckersTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CitiesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CodesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ComparedToCategory",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CompletedT",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ConservationConditions",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CorrectorsTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CountriesDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CountriesHierarchy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CountriesLc",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CountriesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedT",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DataQualityBugsTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DataQualityErrorsTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DataQualityInfoTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DataQualityTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DataQualityWarningsTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DataSources",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DataSourcesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DebugParamSortedLangs",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EditorsTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EmbCodes",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EmbCodesDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EmbCodesOrig",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EmbCodesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EntryDatesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ExpirationDateDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FruitsVegetablesNuts100GEstimate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GenericName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageFrontSmallUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageFrontThumbUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageFrontUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageIngredientsSmallUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageIngredientsThumbUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageIngredientsUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageNutritionSmallUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageNutritionThumbUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageNutritionUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageSmallUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageThumbUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "InformersTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IngredientsAnalysisTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IngredientsDebug",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IngredientsFromOrThatMayBeFromPalmOilN",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IngredientsFromPalmOilN",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IngredientsFromPalmOilTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IngredientsHierarchy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IngredientsIdsDebug",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IngredientsN",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IngredientsNTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IngredientsOriginalTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IngredientsTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IngredientsText",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IngredientsTextDebug",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IngredientsTextWithAllergens",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IngredientsThatMayBeFromPalmOilN",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IngredientsThatMayBeFromPalmOilTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "InterfaceVersionCreated",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "InterfaceVersionModified",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "KnownIngredientsN",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LabelsDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LabelsHierarchy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LabelsLc",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LabelsPrevHierarchy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LabelsPrevTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LabelsTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LangDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LanguagesCodesEn",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LanguagesCodesFr",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LanguagesCodesPl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LanguagesHierarchy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LanguagesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastEditDatesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastEditor",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastImageDatesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastImageT",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastModifiedT",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LinkDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ManufacturingPlaces",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ManufacturingPlacesDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ManufacturingPlacesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MaxImgid",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MineralsPrevTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MineralsTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MiscTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NetWeightUnit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NetWeightValue",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NoNutritionData",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NovaGroup",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NovaGroupDebug",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NovaGroupTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NovaGroups",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NovaGroupsTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NucleotidesPrevTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NucleotidesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NutrientLevelsFat",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NutrientLevelsSalt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NutrientLevelsSaturatedFat",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NutrientLevelsSugars",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NutrientLevelsTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NutritionData",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NutritionDataPer",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NutritionDataPerDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NutritionDataPrepared",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NutritionDataPreparedPer",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NutritionGrades",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NutritionGradesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NutritionScoreBeverage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NutritionScoreDebug",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NutritionScoreWarningNoFiber",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NutritionScoreWarningNoFruitsVegetablesNuts",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OriginsDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OriginsTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OtherInformation",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OtherNutritionalSubstancesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PackagingDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PackagingTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PhotographersTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PnnsGroups1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PnnsGroups1Tags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PnnsGroups2",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PnnsGroups2Tags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PopularityKey",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProducerVersionId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductQuantity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PurchasePlaces",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PurchasePlacesDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PurchasePlacesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "QualityTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "QuantityDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RecyclingInstructionsToDiscard",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ServingQuantity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ServingSize",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ServingSizeDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StatesHierarchy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StatesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StoresDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StoresTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TracesDebugTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TracesFromIngredients",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TracesFromUser",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TracesHierarchy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TracesLc",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TracesTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnknownIngredientsN",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnknownNutrientsTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UpdateKey",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VitaminsPrevTags",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VitaminsTags",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Nutrients_Carbohydrates",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Nutrients_EnergyKcal",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Nutrients_Fat",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Nutrients_Proteins",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Nutrients_Salt",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Nutrients_SaturatedFat",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Nutrients_Sugars",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrontImageUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Nutrients_Carbohydrates",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Nutrients_EnergyKcal",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Nutrients_Fat",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Nutrients_Proteins",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Nutrients_Salt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Nutrients_SaturatedFat",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Nutrients_Sugars",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AdditivesDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdditivesN",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdditivesOldN",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditivesOldTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditivesOriginalTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditivesPrevOriginalTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditivesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AllergensFromIngredients",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AllergensFromUser",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AllergensHierarchy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AllergensLc",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AllergensTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AminoAcidsPrevTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AminoAcidsTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brands",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrandsDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrandsTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarbonFootprintFromKnownIngredientsDebug",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarbonFootprintPercentOfKnownIngredients",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoriesHierarchy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoriesLc",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoriesPropertiesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoriesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckersTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CitiesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComparedToCategory",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CompletedT",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConservationConditions",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorrectorsTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountriesDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountriesHierarchy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountriesLc",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountriesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedT",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataQualityBugsTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataQualityErrorsTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataQualityInfoTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataQualityTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataQualityWarningsTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataSources",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataSourcesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DebugParamSortedLangs",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditorsTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmbCodes",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmbCodesDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmbCodesOrig",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmbCodesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntryDatesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpirationDate",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpirationDateDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FruitsVegetablesNuts100GEstimate",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GenericName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFrontSmallUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFrontThumbUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFrontUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageIngredientsSmallUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageIngredientsThumbUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageIngredientsUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageNutritionSmallUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageNutritionThumbUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageNutritionUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageSmallUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageThumbUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InformersTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IngredientsAnalysisTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IngredientsDebug",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IngredientsFromOrThatMayBeFromPalmOilN",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IngredientsFromPalmOilN",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IngredientsFromPalmOilTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IngredientsHierarchy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IngredientsIdsDebug",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IngredientsN",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IngredientsNTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IngredientsOriginalTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IngredientsTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IngredientsText",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IngredientsTextDebug",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IngredientsTextWithAllergens",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IngredientsThatMayBeFromPalmOilN",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IngredientsThatMayBeFromPalmOilTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InterfaceVersionCreated",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InterfaceVersionModified",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KnownIngredientsN",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabelsDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabelsHierarchy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabelsLc",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabelsPrevHierarchy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabelsPrevTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabelsTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LangDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguagesCodesEn",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguagesCodesFr",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguagesCodesPl",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguagesHierarchy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguagesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastEditDatesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastEditor",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastImageDatesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastImageT",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifiedT",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManufacturingPlaces",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManufacturingPlacesDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManufacturingPlacesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaxImgid",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MineralsPrevTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MineralsTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiscTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NetWeightUnit",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NetWeightValue",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoNutritionData",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NovaGroup",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NovaGroupDebug",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NovaGroupTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NovaGroups",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NovaGroupsTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NucleotidesPrevTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NucleotidesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NutrientLevelsFat",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NutrientLevelsSalt",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NutrientLevelsSaturatedFat",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NutrientLevelsSugars",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NutrientLevelsTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NutritionData",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NutritionDataPer",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NutritionDataPerDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NutritionDataPrepared",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NutritionDataPreparedPer",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NutritionGrades",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NutritionGradesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NutritionScoreBeverage",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NutritionScoreDebug",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NutritionScoreWarningNoFiber",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NutritionScoreWarningNoFruitsVegetablesNuts",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginsDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginsTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherInformation",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherNutritionalSubstancesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackagingDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackagingTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotographersTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PnnsGroups1",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PnnsGroups1Tags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PnnsGroups2",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PnnsGroups2Tags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PopularityKey",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProducerVersionId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductQuantity",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchasePlaces",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchasePlacesDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchasePlacesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QualityTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuantityDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecyclingInstructionsToDiscard",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServingQuantity",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServingSize",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServingSizeDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatesHierarchy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoresDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoresTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TracesDebugTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TracesFromIngredients",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TracesFromUser",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TracesHierarchy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TracesLc",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TracesTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnknownIngredientsN",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnknownNutrientsTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateKey",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VitaminsPrevTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VitaminsTags",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LanguagesCodes",
                columns: table => new
                {
                    En = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Fr = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Pl = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguagesCodes", x => new { x.En, x.Fr, x.Pl });
                });

            migrationBuilder.CreateTable(
                name: "NutrientLevels",
                columns: table => new
                {
                    Salt = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Sugars = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SaturatedFat = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Fat = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutrientLevels", x => new { x.Salt, x.Sugars, x.SaturatedFat, x.Fat });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_LanguagesCodesEn_LanguagesCodesFr_LanguagesCodesPl",
                table: "Products",
                columns: new[] { "LanguagesCodesEn", "LanguagesCodesFr", "LanguagesCodesPl" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_NutrientLevelsSalt_NutrientLevelsSugars_NutrientLevelsSaturatedFat_NutrientLevelsFat",
                table: "Products",
                columns: new[] { "NutrientLevelsSalt", "NutrientLevelsSugars", "NutrientLevelsSaturatedFat", "NutrientLevelsFat" });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_LanguagesCodes_LanguagesCodesEn_LanguagesCodesFr_LanguagesCodesPl",
                table: "Products",
                columns: new[] { "LanguagesCodesEn", "LanguagesCodesFr", "LanguagesCodesPl" },
                principalTable: "LanguagesCodes",
                principalColumns: new[] { "En", "Fr", "Pl" });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_NutrientLevels_NutrientLevelsSalt_NutrientLevelsSugars_NutrientLevelsSaturatedFat_NutrientLevelsFat",
                table: "Products",
                columns: new[] { "NutrientLevelsSalt", "NutrientLevelsSugars", "NutrientLevelsSaturatedFat", "NutrientLevelsFat" },
                principalTable: "NutrientLevels",
                principalColumns: new[] { "Salt", "Sugars", "SaturatedFat", "Fat" });
        }
    }
}

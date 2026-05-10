using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodSense.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    Fat = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SaturatedFat = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Sugars = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutrientLevels", x => new { x.Salt, x.Sugars, x.SaturatedFat, x.Fat });
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LanguagesCodesEn = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LanguagesCodesFr = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LanguagesCodesPl = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NutrientLevelsSalt = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NutrientLevelsSugars = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NutrientLevelsSaturatedFat = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NutrientLevelsFat = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AdditivesN = table.Column<int>(type: "int", nullable: true),
                    AdditivesOldN = table.Column<int>(type: "int", nullable: true),
                    AdditivesOriginalTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditivesOldTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditivesPrevOriginalTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditivesDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditivesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllergensFromIngredients = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllergensFromUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllergensHierarchy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllergensLc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllergensTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AminoAcidsPrevTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AminoAcidsTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brands = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandsDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandsTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarbonFootprintPercentOfKnownIngredients = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarbonFootprintFromKnownIngredientsDebug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoriesHierarchy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoriesLc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoriesPropertiesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoriesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckersTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CitiesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComparedToCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedT = table.Column<long>(type: "bigint", nullable: true),
                    ConservationConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountriesHierarchy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountriesLc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountriesDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountriesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectorsTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedT = table.Column<long>(type: "bigint", nullable: true),
                    DataQualityBugsTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataQualityErrorsTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataQualityInfoTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataQualityTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataQualityWarningsTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataSources = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataSourcesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebugParamSortedLangs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EditorsTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmbCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmbCodesDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmbCodesOrig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmbCodesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryDatesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpirationDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpirationDateDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FruitsVegetablesNuts100GEstimate = table.Column<int>(type: "int", nullable: true),
                    GenericName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageFrontSmallUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageFrontThumbUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageFrontUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageIngredientsUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageIngredientsSmallUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageIngredientsThumbUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageNutritionSmallUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageNutritionThumbUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageNutritionUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageSmallUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageThumbUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InformersTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngredientsAnalysisTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngredientsDebug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngredientsFromOrThatMayBeFromPalmOilN = table.Column<int>(type: "int", nullable: true),
                    IngredientsFromPalmOilTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngredientsFromPalmOilN = table.Column<int>(type: "int", nullable: true),
                    IngredientsHierarchy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngredientsIdsDebug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngredientsN = table.Column<int>(type: "int", nullable: true),
                    IngredientsNTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngredientsOriginalTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngredientsTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngredientsText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngredientsTextDebug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngredientsTextWithAllergens = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngredientsThatMayBeFromPalmOilN = table.Column<int>(type: "int", nullable: true),
                    IngredientsThatMayBeFromPalmOilTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterfaceVersionCreated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterfaceVersionModified = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KnownIngredientsN = table.Column<int>(type: "int", nullable: true),
                    LabelsHierarchy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabelsLc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabelsPrevHierarchy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabelsPrevTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabelsTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabelsDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguagesHierarchy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguagesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastEditDatesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastEditor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastImageDatesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastImageT = table.Column<long>(type: "bigint", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedT = table.Column<long>(type: "bigint", nullable: true),
                    LinkDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManufacturingPlaces = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManufacturingPlacesDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManufacturingPlacesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxImgid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MineralsPrevTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MineralsTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiscTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetWeightUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetWeightValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NutritionDataPer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NutritionScoreWarningNoFruitsVegetablesNuts = table.Column<int>(type: "int", nullable: true),
                    NoNutritionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NovaGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NovaGroups = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NovaGroupDebug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NovaGroupTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NovaGroupsTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NucleotidesPrevTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NucleotidesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NutrientLevelsTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NutritionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NutritionDataPerDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NutritionDataPrepared = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NutritionDataPreparedPer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NutritionGrades = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NutritionScoreBeverage = table.Column<int>(type: "int", nullable: true),
                    NutritionScoreDebug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NutritionScoreWarningNoFiber = table.Column<int>(type: "int", nullable: true),
                    NutritionGradesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginsDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginsTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherNutritionalSubstancesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackagingDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackagingTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotographersTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PnnsGroups1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PnnsGroups2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PnnsGroups1Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PnnsGroups2Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PopularityKey = table.Column<long>(type: "bigint", nullable: true),
                    ProducerVersionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductQuantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchasePlaces = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchasePlacesDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchasePlacesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QualityTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecyclingInstructionsToDiscard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServingQuantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServingSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServingSizeDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatesHierarchy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoresDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoresTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TracesFromIngredients = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TracesHierarchy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TracesDebugTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TracesFromUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TracesLc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TracesTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnknownIngredientsN = table.Column<int>(type: "int", nullable: true),
                    UnknownNutrientsTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VitaminsPrevTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VitaminsTags = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.UniqueConstraint("AK_Products_Barcode", x => x.Barcode);
                    table.ForeignKey(
                        name: "FK_Products_LanguagesCodes_LanguagesCodesEn_LanguagesCodesFr_LanguagesCodesPl",
                        columns: x => new { x.LanguagesCodesEn, x.LanguagesCodesFr, x.LanguagesCodesPl },
                        principalTable: "LanguagesCodes",
                        principalColumns: new[] { "En", "Fr", "Pl" });
                    table.ForeignKey(
                        name: "FK_Products_NutrientLevels_NutrientLevelsSalt_NutrientLevelsSugars_NutrientLevelsSaturatedFat_NutrientLevelsFat",
                        columns: x => new { x.NutrientLevelsSalt, x.NutrientLevelsSugars, x.NutrientLevelsSaturatedFat, x.NutrientLevelsFat },
                        principalTable: "NutrientLevels",
                        principalColumns: new[] { "Salt", "Sugars", "SaturatedFat", "Fat" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_LanguagesCodesEn_LanguagesCodesFr_LanguagesCodesPl",
                table: "Products",
                columns: new[] { "LanguagesCodesEn", "LanguagesCodesFr", "LanguagesCodesPl" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_NutrientLevelsSalt_NutrientLevelsSugars_NutrientLevelsSaturatedFat_NutrientLevelsFat",
                table: "Products",
                columns: new[] { "NutrientLevelsSalt", "NutrientLevelsSugars", "NutrientLevelsSaturatedFat", "NutrientLevelsFat" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "LanguagesCodes");

            migrationBuilder.DropTable(
                name: "NutrientLevels");
        }
    }
}

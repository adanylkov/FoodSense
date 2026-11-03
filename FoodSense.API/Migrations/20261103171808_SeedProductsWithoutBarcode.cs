using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodSense.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductsWithoutBarcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Barcode", "CategoryId", "FrontImageUrl", "Name", "Nutrients_Carbohydrates", "Nutrients_Fat", "Nutrients_Kcal", "Nutrients_Proteins", "Nutrients_Salt", "Nutrients_SaturatedFat", "Nutrients_Sugars" },
                values: new object[,]
                {
                    { 2001, null, 1, "", "Jabłko", 13.800000000000001, 0.10000000000000001, 52.0, 0.29999999999999999, 0.0, 0.0, 10.4 },
                    { 2002, null, 1, "", "Banan", 23.0, 0.29999999999999999, 89.0, 1.1000000000000001, 0.0, 0.10000000000000001, 12.199999999999999 },
                    { 2003, null, 1, "", "Gruszka", 15.0, 0.10000000000000001, 57.0, 0.40000000000000002, 0.0, 0.0, 9.6999999999999993 },
                    { 2004, null, 1, "", "Pomarańcza", 11.800000000000001, 0.10000000000000001, 47.0, 0.80000000000000004, 0.0, 0.0, 9.4000000000000004 },
                    { 2005, null, 1, "", "Mandarynka", 11.5, 0.20000000000000001, 45.0, 0.80000000000000004, 0.0, 0.0, 9.0999999999999996 },
                    { 2006, null, 1, "", "Truskawki", 8.0, 0.29999999999999999, 33.0, 0.69999999999999996, 0.0, 0.10000000000000001, 4.9000000000000004 },
                    { 2007, null, 1, "", "Maliny", 12.0, 0.69999999999999996, 52.0, 1.2, 0.0, 0.10000000000000001, 11.199999999999999 },
                    { 2008, null, 1, "", "Borówka amerykańska", 12.0, 0.5, 46.0, 0.40000000000000002, 0.0, 0.0, 10.0 },
                    { 2009, null, 1, "", "Czereśnie", 16.0, 0.20000000000000001, 63.0, 1.1000000000000001, 0.0, 0.0, 12.0 },
                    { 2010, null, 1, "", "Śliwka", 11.4, 0.29999999999999999, 46.0, 0.69999999999999996, 0.0, 0.0, 9.4000000000000004 },
                    { 2011, null, 1, "", "Winogrona", 18.100000000000001, 0.40000000000000002, 69.0, 0.69999999999999996, 0.0, 0.0, 15.5 },
                    { 2012, null, 1, "", "Kiwi", 14.699999999999999, 0.69999999999999996, 61.0, 1.1000000000000001, 0.0, 0.0, 9.0 },
                    { 2013, null, 1, "", "Mango", 15.0, 0.40000000000000002, 60.0, 0.80000000000000004, 0.0, 0.0, 13.699999999999999 },
                    { 2014, null, 1, "", "Awokado", 8.5, 14.699999999999999, 160.0, 2.0, 0.0, 2.2000000000000002, 0.69999999999999996 },
                    { 2015, null, 1, "", "Melon", 8.1999999999999993, 0.20000000000000001, 34.0, 0.80000000000000004, 0.0, 0.0, 7.9000000000000004 },
                    { 2016, null, 1, "", "Granat", 18.699999999999999, 1.2, 83.0, 1.7, 0.0, 0.0, 13.699999999999999 },
                    { 2017, null, 1, "", "Morela", 3.8999999999999999, 0.40000000000000002, 15.0, 0.5, 0.0, 0.0, 3.2000000000000002 },
                    { 2018, null, 1, "", "Brzoskwinia", 9.5, 0.29999999999999999, 39.0, 0.90000000000000002, 0.0, 0.0, 8.8000000000000007 },
                    { 2101, null, 2, "", "Ziemniaki", 20.100000000000001, 0.10000000000000001, 86.0, 2.0, 0.0, 0.0, 0.90000000000000002 },
                    { 2102, null, 2, "", "Pomidor", 3.8999999999999999, 0.20000000000000001, 18.0, 0.80000000000000004, 0.0, 0.0, 2.6000000000000001 },
                    { 2103, null, 2, "", "Ogórek", 3.6000000000000001, 0.10000000000000001, 15.0, 0.69999999999999996, 0.0, 0.0, 1.7 },
                    { 2104, null, 2, "", "Marchew", 9.5999999999999996, 0.90000000000000002, 41.0, 0.90000000000000002, 0.0, 0.0, 4.7000000000000002 },
                    { 2105, null, 2, "", "Cebula", 9.3000000000000007, 0.10000000000000001, 40.0, 1.1000000000000001, 0.0, 0.0, 4.2000000000000002 },
                    { 2106, null, 2, "", "Czosnek", 33.100000000000001, 0.5, 149.0, 6.4000000000000004, 0.0, 0.0, 1.0 },
                    { 2107, null, 2, "", "Por", 14.4, 0.29999999999999999, 88.0, 1.3999999999999999, 0.0, 0.0, 1.5 },
                    { 2108, null, 2, "", "Kapusta biała", 5.7999999999999998, 0.10000000000000001, 25.0, 1.3, 0.0, 0.0, 3.2000000000000002 },
                    { 2109, null, 2, "", "Kapusta czerwona", 5.5999999999999996, 0.10000000000000001, 31.0, 1.3999999999999999, 0.0, 0.0, 3.2000000000000002 },
                    { 2110, null, 2, "", "Kalafior", 5.0, 0.29999999999999999, 25.0, 2.3999999999999999, 0.0, 0.0, 1.8999999999999999 },
                    { 2111, null, 2, "", "Brokuł", 7.0, 0.40000000000000002, 34.0, 2.7999999999999998, 0.0, 0.0, 1.3999999999999999 },
                    { 2112, null, 2, "", "Cukinia", 3.1000000000000001, 0.0, 17.0, 1.2, 0.0, 0.0, 2.5 },
                    { 2113, null, 2, "", "Sałata", 2.8999999999999999, 0.20000000000000001, 15.0, 1.3, 0.0, 0.0, 1.3 },
                    { 2114, null, 2, "", "Papryka czerwona", 6.0, 0.29999999999999999, 31.0, 1.0, 0.0, 0.0, 3.5 },
                    { 2115, null, 2, "", "Zielony groszek", 14.5, 0.5, 81.0, 5.4000000000000004, 0.0, 0.0, 5.0999999999999996 },
                    { 2116, null, 2, "", "Szpinak", 3.6000000000000001, 0.40000000000000002, 23.0, 2.8999999999999999, 0.0, 0.0, 0.40000000000000002 },
                    { 2117, null, 2, "", "Szparagi", 3.8999999999999999, 0.10000000000000001, 20.0, 2.0, 0.0, 0.0, 1.8999999999999999 },
                    { 2118, null, 2, "", "Bataty", 20.100000000000001, 0.10000000000000001, 86.0, 1.7, 0.0, 0.0, 4.2000000000000002 },
                    { 2119, null, 2, "", "Dynia", 6.5, 0.10000000000000001, 26.0, 1.0, 0.0, 0.0, 3.0 },
                    { 2120, null, 2, "", "Burak", 9.5999999999999996, 0.20000000000000001, 43.0, 1.6000000000000001, 0.0, 0.0, 8.0 },
                    { 2121, null, 2, "", "Seler", 9.1999999999999993, 0.29999999999999999, 26.0, 1.5, 0.0, 0.0, 0.90000000000000002 },
                    { 2122, null, 2, "", "Seler naciowy", 3.0, 0.20000000000000001, 18.0, 0.69999999999999996, 0.10000000000000001, 0.0, 1.3 },
                    { 2123, null, 2, "", "Pietruszka korzeń", 9.4000000000000004, 0.80000000000000004, 42.0, 3.0, 0.0, 0.0, 3.3999999999999999 },
                    { 2124, null, 2, "", "Kukurydza", 22.800000000000001, 1.3999999999999999, 86.0, 3.2999999999999998, 0.0, 0.0, 12.4 },
                    { 2125, null, 2, "", "Imbir", 15.0, 0.80000000000000004, 79.0, 1.8, 0.0, 0.0, 1.7 },
                    { 2126, null, 2, "", "Szczypiorek", 4.4000000000000004, 0.69999999999999996, 30.0, 3.0, 0.0, 0.0, 1.1000000000000001 },
                    { 2127, null, 2, "", "Koperek", 5.7000000000000002, 1.0, 31.0, 3.0, 0.0, 0.0, 1.7 },
                    { 2201, null, 3, "", "Ryż biały", 80.0, 0.59999999999999998, 360.0, 7.0, 0.0, 0.20000000000000001, 0.5 },
                    { 2202, null, 3, "", "Płatki owsiane", 66.299999999999997, 6.9000000000000004, 389.0, 16.899999999999999, 0.0, 1.6000000000000001, 1.0 },
                    { 2203, null, 3, "", "Kasza gryczana", 71.5, 3.3999999999999999, 347.0, 13.300000000000001, 0.0, 0.59999999999999998, 1.2 },
                    { 2204, null, 3, "", "Makaron", 75.0, 1.5, 371.0, 13.0, 0.0, 0.29999999999999999, 2.5 },
                    { 2205, null, 3, "", "Mąka pszenna", 73.0, 1.2, 364.0, 10.9, 0.0, 0.20000000000000001, 3.1000000000000001 },
                    { 2206, null, 3, "", "Chleb", 49.0, 3.2000000000000002, 265.0, 9.0, 1.0, 0.69999999999999996, 4.2999999999999998 },
                    { 2301, null, 4, "", "Mleko 3,2%", 4.7999999999999998, 3.2000000000000002, 64.0, 3.2000000000000002, 0.10000000000000001, 2.0, 4.7999999999999998 },
                    { 2302, null, 4, "", "Śmietana 18%", 4.2000000000000002, 18.0, 200.0, 2.6000000000000001, 0.10000000000000001, 10.4, 3.3999999999999999 },
                    { 2303, null, 4, "", "Jogurt naturalny", 4.7000000000000002, 3.2999999999999998, 61.0, 3.5, 0.10000000000000001, 2.1000000000000001, 4.7000000000000002 },
                    { 2304, null, 4, "", "Masło", 0.69999999999999996, 81.099999999999994, 743.0, 0.90000000000000002, 0.0, 51.399999999999999, 0.59999999999999998 },
                    { 2401, null, 8, "", "Jajko", 0.69999999999999996, 9.5, 143.0, 12.6, 0.29999999999999999, 3.1000000000000001, 0.40000000000000002 },
                    { 2402, null, 8, "", "Pieczarki", 3.2999999999999998, 0.20000000000000001, 22.0, 3.1000000000000001, 0.0, 0.0, 2.0 },
                    { 2403, null, 8, "", "Oliwa z oliwek", 0.0, 99.900000000000006, 884.0, 0.0, 0.0, 13.800000000000001, 0.0 },
                    { 2404, null, 8, "", "Cukier", 99.900000000000006, 0.0, 387.0, 0.0, 0.0, 0.0, 99.900000000000006 },
                    { 2405, null, 8, "", "Miód", 82.400000000000006, 0.0, 304.0, 0.29999999999999999, 0.0, 0.0, 82.099999999999994 },
                    { 2406, null, 8, "", "Orzechy włoskie", 13.699999999999999, 65.200000000000003, 654.0, 15.199999999999999, 0.0, 6.0999999999999996, 2.6000000000000001 },
                    { 2407, null, 8, "", "Migdały", 21.199999999999999, 49.899999999999999, 579.0, 21.199999999999999, 0.0, 3.7999999999999998, 3.3999999999999999 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2001);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2002);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2003);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2004);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2005);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2006);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2007);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2008);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2009);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2010);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2011);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2012);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2013);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2014);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2015);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2016);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2017);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2018);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2101);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2102);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2103);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2104);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2105);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2106);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2107);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2108);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2109);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2110);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2111);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2112);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2113);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2114);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2115);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2116);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2117);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2118);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2119);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2120);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2121);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2122);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2123);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2124);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2125);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2126);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2127);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2201);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2202);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2203);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2204);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2205);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2206);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2301);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2302);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2303);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2304);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2401);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2402);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2403);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2404);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2405);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2406);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2407);
        }
    }
}

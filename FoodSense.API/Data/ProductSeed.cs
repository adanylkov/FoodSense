using FoodSense.API.Data.Models;

namespace FoodSense.API.Data;

/// <summary>Seed data for everyday products sold without a barcode (produce, eggs, bulk goods).
/// Nutrient values are per 100 g of the product in its raw/uncooked state, taken from USDA
/// FoodData Central and Polish food composition tables. Ids live in a dedicated range
/// (2001+) so they never collide with products persisted at runtime (ids start at 1055).
/// EF Core cannot seed through the owned navigation <c>Nutrients</c>, so nutrients are seeded
/// as rows of the owned type keyed by <c>ProductId</c>.</summary>
public static class ProductSeed
{
    public static Product[] Data => [
        // Fruits
        P(2001, "Jabłko", 1), P(2002, "Banan", 1), P(2003, "Gruszka", 1),
        P(2004, "Pomarańcza", 1), P(2005, "Mandarynka", 1), P(2006, "Truskawki", 1),
        P(2007, "Maliny", 1), P(2008, "Borówka amerykańska", 1), P(2009, "Czereśnie", 1),
        P(2010, "Śliwka", 1), P(2011, "Winogrona", 1), P(2012, "Kiwi", 1),
        P(2013, "Mango", 1), P(2014, "Awokado", 1), P(2015, "Melon", 1),
        P(2016, "Granat", 1), P(2017, "Morela", 1), P(2018, "Brzoskwinia", 1),

        // Vegetables
        P(2101, "Ziemniaki", 2), P(2102, "Pomidor", 2), P(2103, "Ogórek", 2),
        P(2104, "Marchew", 2), P(2105, "Cebula", 2), P(2106, "Czosnek", 2),
        P(2107, "Por", 2), P(2108, "Kapusta biała", 2), P(2109, "Kapusta czerwona", 2),
        P(2110, "Kalafior", 2), P(2111, "Brokuł", 2), P(2112, "Cukinia", 2),
        P(2113, "Sałata", 2), P(2114, "Papryka czerwona", 2), P(2115, "Zielony groszek", 2),
        P(2116, "Szpinak", 2), P(2117, "Szparagi", 2), P(2118, "Bataty", 2),
        P(2119, "Dynia", 2), P(2120, "Burak", 2), P(2121, "Seler", 2),
        P(2122, "Seler naciowy", 2), P(2123, "Pietruszka korzeń", 2), P(2124, "Kukurydza", 2),
        P(2125, "Imbir", 2), P(2126, "Szczypiorek", 2), P(2127, "Koperek", 2),

        // Grains
        P(2201, "Ryż biały", 3), P(2202, "Płatki owsiane", 3), P(2203, "Kasza gryczana", 3),
        P(2204, "Makaron", 3), P(2205, "Mąka pszenna", 3), P(2206, "Chleb", 3),

        // Dairy
        P(2301, "Mleko 3,2%", 4), P(2302, "Śmietana 18%", 4),
        P(2303, "Jogurt naturalny", 4), P(2304, "Masło", 4),

        // Other
        P(2401, "Jajko", 8), P(2402, "Pieczarki", 8), P(2403, "Oliwa z oliwek", 8),
        P(2404, "Cukier", 8), P(2405, "Miód", 8), P(2406, "Orzechy włoskie", 8),
        P(2407, "Migdały", 8)
    ];

    /// <summary>Rows of the owned type <c>Nutrients</c>, keyed by <c>ProductId</c>.</summary>
    public static NutrientsSeed[] NutrientsData => [
        N(2001, 52, 0.1, 0.0, 13.8, 10.4, 0.3, 0.0), N(2002, 89, 0.3, 0.1, 23.0, 12.2, 1.1, 0.0),
        N(2003, 57, 0.1, 0.0, 15.0, 9.7, 0.4, 0.0), N(2004, 47, 0.1, 0.0, 11.8, 9.4, 0.8, 0.0),
        N(2005, 45, 0.2, 0.0, 11.5, 9.1, 0.8, 0.0), N(2006, 33, 0.3, 0.1, 8.0, 4.9, 0.7, 0.0),
        N(2007, 52, 0.7, 0.1, 12.0, 11.2, 1.2, 0.0), N(2008, 46, 0.5, 0.0, 12.0, 10.0, 0.4, 0.0),
        N(2009, 63, 0.2, 0.0, 16.0, 12.0, 1.1, 0.0), N(2010, 46, 0.3, 0.0, 11.4, 9.4, 0.7, 0.0),
        N(2011, 69, 0.4, 0.0, 18.1, 15.5, 0.7, 0.0), N(2012, 61, 0.7, 0.0, 14.7, 9.0, 1.1, 0.0),
        N(2013, 60, 0.4, 0.0, 15.0, 13.7, 0.8, 0.0), N(2014, 160, 14.7, 2.2, 8.5, 0.7, 2.0, 0.0),
        N(2015, 34, 0.2, 0.0, 8.2, 7.9, 0.8, 0.0), N(2016, 83, 1.2, 0.0, 18.7, 13.7, 1.7, 0.0),
        N(2017, 15, 0.4, 0.0, 3.9, 3.2, 0.5, 0.0), N(2018, 39, 0.3, 0.0, 9.5, 8.8, 0.9, 0.0),
        N(2101, 86, 0.1, 0.0, 20.1, 0.9, 2.0, 0.0), N(2102, 18, 0.2, 0.0, 3.9, 2.6, 0.8, 0.0),
        N(2103, 15, 0.1, 0.0, 3.6, 1.7, 0.7, 0.0), N(2104, 41, 0.9, 0.0, 9.6, 4.7, 0.9, 0.0),
        N(2105, 40, 0.1, 0.0, 9.3, 4.2, 1.1, 0.0), N(2106, 149, 0.5, 0.0, 33.1, 1.0, 6.4, 0.0),
        N(2107, 88, 0.3, 0.0, 14.4, 1.5, 1.4, 0.0), N(2108, 25, 0.1, 0.0, 5.8, 3.2, 1.3, 0.0),
        N(2109, 31, 0.1, 0.0, 5.6, 3.2, 1.4, 0.0), N(2110, 25, 0.3, 0.0, 5.0, 1.9, 2.4, 0.0),
        N(2111, 34, 0.4, 0.0, 7.0, 1.4, 2.8, 0.0), N(2112, 17, 0.0, 0.0, 3.1, 2.5, 1.2, 0.0),
        N(2113, 15, 0.2, 0.0, 2.9, 1.3, 1.3, 0.0), N(2114, 31, 0.3, 0.0, 6.0, 3.5, 1.0, 0.0),
        N(2115, 81, 0.5, 0.0, 14.5, 5.1, 5.4, 0.0), N(2116, 23, 0.4, 0.0, 3.6, 0.4, 2.9, 0.0),
        N(2117, 20, 0.1, 0.0, 3.9, 1.9, 2.0, 0.0), N(2118, 86, 0.1, 0.0, 20.1, 4.2, 1.7, 0.0),
        N(2119, 26, 0.1, 0.0, 6.5, 3.0, 1.0, 0.0), N(2120, 43, 0.2, 0.0, 9.6, 8.0, 1.6, 0.0),
        N(2121, 26, 0.3, 0.0, 9.2, 0.9, 1.5, 0.0), N(2122, 18, 0.2, 0.0, 3.0, 1.3, 0.7, 0.1),
        N(2123, 42, 0.8, 0.0, 9.4, 3.4, 3.0, 0.0), N(2124, 86, 1.4, 0.0, 22.8, 12.4, 3.3, 0.0),
        N(2125, 79, 0.8, 0.0, 15.0, 1.7, 1.8, 0.0), N(2126, 30, 0.7, 0.0, 4.4, 1.1, 3.0, 0.0),
        N(2127, 31, 1.0, 0.0, 5.7, 1.7, 3.0, 0.0),
        N(2201, 360, 0.6, 0.2, 80.0, 0.5, 7.0, 0.0), N(2202, 389, 6.9, 1.6, 66.3, 1.0, 16.9, 0.0),
        N(2203, 347, 3.4, 0.6, 71.5, 1.2, 13.3, 0.0), N(2204, 371, 1.5, 0.3, 75.0, 2.5, 13.0, 0.0),
        N(2205, 364, 1.2, 0.2, 73.0, 3.1, 10.9, 0.0), N(2206, 265, 3.2, 0.7, 49.0, 4.3, 9.0, 1.0),
        N(2301, 64, 3.2, 2.0, 4.8, 4.8, 3.2, 0.1), N(2302, 200, 18.0, 10.4, 4.2, 3.4, 2.6, 0.1),
        N(2303, 61, 3.3, 2.1, 4.7, 4.7, 3.5, 0.1), N(2304, 743, 81.1, 51.4, 0.7, 0.6, 0.9, 0.0),
        N(2401, 143, 9.5, 3.1, 0.7, 0.4, 12.6, 0.3), N(2402, 22, 0.2, 0.0, 3.3, 2.0, 3.1, 0.0),
        N(2403, 884, 99.9, 13.8, 0.0, 0.0, 0.0, 0.0), N(2404, 387, 0.0, 0.0, 99.9, 99.9, 0.0, 0.0),
        N(2405, 304, 0.0, 0.0, 82.4, 82.1, 0.3, 0.0), N(2406, 654, 65.2, 6.1, 13.7, 2.6, 15.2, 0.0),
        N(2407, 579, 49.9, 3.8, 21.2, 3.4, 21.2, 0.0)
    ];

    private static Product P(int id, string name, int categoryId)
        => new()
        {
            Id = id,
            Barcode = null,
            Name = name,
            FrontImageUrl = string.Empty,
            CategoryId = categoryId,
            // Left null: EF Core seeds the owned Nutrients rows separately.
            Nutrients = null!
        };

    private static NutrientsSeed N(int productId, double kcal, double fat, double saturatedFat,
        double carbohydrates, double sugars, double proteins, double salt)
        => new(productId, kcal, fat, saturatedFat, carbohydrates, sugars, proteins, salt);

    /// <summary>Rows of the owned <c>Nutrients</c> type. The owner key (<c>ProductId</c>) is
    /// required by EF Core when seeding an owned navigation.</summary>
    public record NutrientsSeed(int ProductId, double Kcal, double Fat, double SaturatedFat,
        double Carbohydrates, double Sugars, double Proteins, double Salt);
}

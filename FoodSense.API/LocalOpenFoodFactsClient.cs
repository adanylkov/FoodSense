using FoodSense.API.Data.Models;
using Microsoft.Data.SqlClient;
using System.Globalization;

public sealed class LocalOpenFoodFactsClient(IConfiguration configuration) : IOpenFoodFactsClient
{
    private readonly string _connectionString = configuration.GetConnectionString("openfoodfacts")
        ?? throw new InvalidOperationException(
            "The 'openfoodfacts' connection string is not configured.");

    public async Task<OpenFoodFactProduct?> GetProductByBarcodeAsync(
        string barcode,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT TOP (1)
                Code, ProductName, Brands, Quantity, ImageUrl, ImageSmallUrl,
                EnergyKcal100g, Fat100g, SaturatedFat100g, Carbohydrates100g,
                Sugars100g, Proteins100g, Salt100g
            FROM dbo.Products
            WHERE Code = @barcode;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@barcode", barcode);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken)
            ? ReadProduct(reader)
            : null;
    }

    public async Task<IReadOnlyList<OpenFoodFactProduct>> SearchProductsAsync(
        string query,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var terms = query.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (terms.Length == 0) return [];

        pageSize = Math.Clamp(pageSize, 1, 100);
        var conditions = new List<string>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();

        for (var index = 0; index < terms.Length; index++)
        {
            var parameterName = $"@term{index}";
            conditions.Add($"""
                (ProductName LIKE {parameterName} OR Brands LIKE {parameterName}
                    OR GenericName LIKE {parameterName} OR Code LIKE {parameterName})
                """);
            command.Parameters.AddWithValue(parameterName, $"%{EscapeLikePattern(terms[index])}%");
        }

        command.CommandText = $"""
            SELECT TOP (@limit)
                Code, ProductName, Brands, Quantity, ImageUrl, ImageSmallUrl,
                EnergyKcal100g, Fat100g, SaturatedFat100g, Carbohydrates100g,
                Sugars100g, Proteins100g, Salt100g
            FROM dbo.Products
            WHERE {string.Join(" OR ", conditions)}
            ORDER BY CASE WHEN ProductName IS NULL THEN 1 ELSE 0 END, ProductName, Id;
            """;
        command.Parameters.AddWithValue("@limit", pageSize);

        var products = new List<OpenFoodFactProduct>(pageSize);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            products.Add(ReadProduct(reader));
        }

        return products;
    }

    private static OpenFoodFactProduct ReadProduct(SqlDataReader reader)
        => new()
        {
            Code = ReadString(reader, 0),
            Name = ReadString(reader, 1),
            Brand = ReadString(reader, 2),
            Quantity = ReadString(reader, 3),
            ImageUrl = ReadString(reader, 4),
            ImageSmallUrl = ReadString(reader, 5),
            Nutrients = new OpenFoodFactNutrition
            {
                Calories = ReadDouble(reader, 6),
                Fat = ReadDouble(reader, 7),
                SaturatedFat = ReadDouble(reader, 8),
                Carbohydrates = ReadDouble(reader, 9),
                Sugars = ReadDouble(reader, 10),
                Proteins = ReadDouble(reader, 11),
                Salt = ReadDouble(reader, 12)
            }
        };

    private static string ReadString(SqlDataReader reader, int ordinal)
        => reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);

    private static double ReadDouble(SqlDataReader reader, int ordinal)
        => double.TryParse(
            ReadString(reader, ordinal),
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out var value)
            ? value
            : 0;

    private static string EscapeLikePattern(string value)
        => value.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
}

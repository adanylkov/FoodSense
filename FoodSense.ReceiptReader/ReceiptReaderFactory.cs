namespace FoodSense.ReceiptReader;

public sealed class ReceiptReaderFactory : IReceiptReaderFactory
{
    public IReceiptReader GetReader(string fileExtensionOrType)
    {
        var normalized = (fileExtensionOrType ?? string.Empty).Trim();

        if (normalized.StartsWith(".", StringComparison.Ordinal))
            normalized = normalized[1..];

        return normalized.Equals("json", StringComparison.OrdinalIgnoreCase)
            ? new JPKJsonReceiptReader()
            : throw new NotSupportedException($"Unsupported receipt format '{fileExtensionOrType}'.");
    }
}

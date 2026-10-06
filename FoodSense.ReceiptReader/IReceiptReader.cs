namespace FoodSense.ReceiptReader;

public interface IReceiptReader
{
    Task<IEnumerable<ReceiptItem>> ReadAsync(Stream stream);
}

public interface IReceiptReaderFactory
{
    IReceiptReader GetReader(string fileExtensionOrType);
}


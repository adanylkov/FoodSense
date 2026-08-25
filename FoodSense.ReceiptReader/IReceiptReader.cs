using FoodSense.Common.Models;

namespace FoodSense.ReceiptReader;

public interface IReceiptReader
{
    Task<IEnumerable<ProductDto>> ReadAsync(Stream stream);
}

public interface IReceiptReaderFactory
{
    IReceiptReader GetReader(string fileExtensionOrType);
}


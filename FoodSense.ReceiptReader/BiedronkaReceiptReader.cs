using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;

namespace FoodSense.ReceiptReader;

public class BiedronkaReceiptReader
{
    private static readonly PdfName ImageObject = new("/XObject");
    private static readonly PdfName ImageName = new("/Im0");

    public static IEnumerable<Stream> ReadImages(Stream pdfStream)
    {
        if (PdfReader.TestPdfFile(pdfStream) == 0)
            throw new ArgumentException("Invalid PDF file");

	
        pdfStream.Seek(0, SeekOrigin.Begin);
        var pdfDocument = PdfReader.Open(pdfStream);
        var imageStreams = new List<Stream>(pdfDocument.PageCount);

        foreach (var page in pdfDocument.Pages)
        {
            if (page.Resources.Elements[ImageObject] is PdfDictionary imageObjectDictionary 
                && imageObjectDictionary.Elements[ImageName] is PdfReference { Value: PdfDictionary } imageReference)
            {
                imageStreams.Add(new MemoryStream(((PdfDictionary)imageReference.Value).Stream.Value));
            }
        }

        return imageStreams;
    }
}

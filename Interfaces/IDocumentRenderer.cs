using DocBuilder.Class;

namespace DocBuilder.Interfaces;

public interface IDocumentRenderer
{
    byte[] CreateDocumentPdf(ReportDto details);
    List<byte[]> CreateDocumentImages(ReportDto details);
}
using DocBuilder.Class;
using DocBuilder.Interfaces;

namespace DocBuilder.Services;

public class PdfService : IPdfService
{
    private readonly IDocumentRenderer _documentRenderer;
    public PdfService(IDocumentRenderer documentRenderer)
    {
        _documentRenderer = documentRenderer;
    }
    
    public byte[] CreateDocumentPdf(ReportDto details)
    {
        return _documentRenderer.CreateDocumentPdf(details);
    }
}
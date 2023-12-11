using DocBuilder.Class;

namespace DocBuilder.Interfaces;

public interface IPdfService
{
    byte[] CreateDocumentPdf(ReportDto details);
}
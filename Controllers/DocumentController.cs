using DocBuilder.Class;
using Microsoft.AspNetCore.Mvc;

namespace DocBuilder.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    [HttpPost("pdf")]
    public IActionResult CreatePdf([FromBody] ReportDto request)
    {
        // Logic to create a PDF document
        return Ok(null);
    }

    [HttpPost("xlsx")]
    public IActionResult CreateXlsx([FromBody] XlsxRequest request)
    {
        // Logic to create an XLSX document
        return Ok(null);
    }

    [HttpPost("csv")]
    public IActionResult CreateCsv([FromBody] CsvRequest request)
    {
        // Logic to create a CSV document
        return Ok(null);
    }

    [HttpPost("word")]
    public IActionResult CreateWord([FromBody] WordRequest request)
    {
        // Logic to create a Word document
        return Ok(null);
    }

    [HttpPost("images")]
    public IActionResult CreateDocumentImages([FromBody] ImageRequest request)
    {
        // Logic to create images for each page of the document
        return Ok(null);
    }
}

public class PdfRequest
{
    // Properties specific to PDF generation
}

public class XlsxRequest
{
    // Properties specific to XLSX generation
}

public class CsvRequest
{
    // Properties specific to CSV generation
}

public class WordRequest
{
    // Properties specific to Word generation
}

public class ImageRequest
{
    // Properties specific to image generation
}
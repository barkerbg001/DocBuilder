using DocBuilder.Class;
using DocBuilder.Models;
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
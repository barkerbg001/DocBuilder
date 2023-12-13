using DocBuilder.Class;
using DocBuilder.Enums;
using DocBuilder.Interfaces;
using DocBuilder.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocBuilder.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentSampleController : ControllerBase
{
    private readonly IPdfService _pdfService;

    public DocumentSampleController(IPdfService pdfService)
    {
        _pdfService = pdfService;
    }

    [HttpPost("pdf")]
    public IActionResult CreatePdf()
    {
        try
        {
            var dwdata = new List<ReportDrawDetails>();
            var head = new List<ReportDrawDetails>();
            var foot = new List<ReportDrawDetails>();

            head.Add(new ReportDrawDetails
            {
                ColNo = 1,
                colSpan = 10,
                isNumber = false,
                RowId = 1,
                Value = "Header of the report",
                useCustomFont = true,
                customPaint = new MakePaint
                {
                    HexColor = "#323232",
                    IsAntiAlias = true,
                    Align = Align.Center,
                    IsBold = true,
                    TextSize = 17
                },
            });

            int index = 0;

            dwdata.Add(new ReportDrawDetails
            {
                ColNo = 1,
                colSpan = 10,
                borderBottom = true,
                RowId = index,
                Value = "Body of the report"
            });

            var pdf = new ReportDto
            {
                specs = new ReportDrawSpecs
                {
                    //ColWidth = new List<int> { 120, 110, 240, 80, 100, 100, 100 },
                    ColWidth = new List<int> { 114, 114, 114, 114, 114, 114, 114, 114, 114, 114 },
                    HasPageNum = true,
                    pageNum = new ReportDrawDetails
                    {
                        ColNo = 10,
                        colSpan = 1,
                        isNumber = true,
                        RowId = 1,
                        Value = "Page ? of ?"
                    },
                    Orientation = Orientation.Landscape
                },
                data = dwdata,
                header = head,
                footer = foot
            };
            var pdfContent = _pdfService.CreateDocumentPdf(pdf);
            return File(pdfContent, "application/pdf", "generated-document.pdf");
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., invalid input, generation failure)
            return BadRequest(ex.Message);
        }
    }
}
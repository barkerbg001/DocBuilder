using DocBuilder.Class;
using DocBuilder.Enum;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using SkiaSharp;

namespace DocBuilder.Service
{
    public class DrawReport
    {
        private SKColor skBlack = new SKColor(0, 0, 0);
        private SKColor skWhite = new SKColor(255, 255, 255);

        private int pdfWidth = 900;
        private int pdfHeight = 1188;
        private int rowheight = 20;
        private int gridSpacing = 1;
        private float Padyup = 20;
        private float Padydown = 1188;
        private float Padxleft = 15;
        private float Padxright = 900;
        private int totalPages = 0;

        private SKPaint pntGridLines, pntDataGridBodyLeft, pntDataGridBodyRight;
        private SKFont SKFont, SKBold;
        public DrawReport()
        {
            SetFonts();
        }

        public void SetFonts()
        {
            SKFont = new SKFont(SKTypeface.FromFamilyName("Helvetica", SKFontStyleWeight.Medium, SKFontStyleWidth.Condensed, SKFontStyleSlant.Upright));
            SKBold = new SKFont(SKTypeface.FromFamilyName("Helvetica", SKFontStyleWeight.Bold, SKFontStyleWidth.Condensed, SKFontStyleSlant.Upright));
            pntGridLines = MakeSKPaint(skBlack, true, 20.0f, SKTextAlign.Center);
            pntDataGridBodyLeft = MakeSKPaint(skBlack, true, 11, SKTextAlign.Left);
            pntDataGridBodyRight = MakeSKPaint(skBlack, true, 11, SKTextAlign.Right);
        }

        public DocumentBuilder CreateReport(ReportDto details)
        {
            var specs = details.specs;
            var header = details.header;
            var footer = details.footer;
            var data = details.data;
            pdfHeight = specs.Orientation == Orientation.Portrait ? 1188 : 900;
            Padydown = pdfHeight;
            pdfWidth = specs.Orientation == Orientation.Portrait ? 900 : 1188;
            Padxright = pdfWidth;
            var pdfBulderClass = new DocumentBuilder();
            var dataImages = data.Select(x => new ReportDrawDetails
            {
                borderBottom = x.borderBottom,
                borderLeft = x.borderLeft,
                borderRight = x.borderRight,
                borderTop = x.borderTop,
                Centre = x.Centre,
                ColNo = x.ColNo,
                colSpan = x.colSpan,
                customPaint = x.customPaint,
                image = x.image,
                isImage = x.isImage,
                isNumber = x.isNumber,
                RowId = x.RowId,
                useCustomFont = x.useCustomFont,
                Value = x.Value,
                backgroundColorPaint = x.backgroundColorPaint,
                hasBackgroundColor = x.hasBackgroundColor,
            }).ToList();
            var dataPDF = data.Select(x => new ReportDrawDetails
            {
                borderBottom = x.borderBottom,
                borderLeft = x.borderLeft,
                borderRight = x.borderRight,
                borderTop = x.borderTop,
                Centre = x.Centre,
                ColNo = x.ColNo,
                colSpan = x.colSpan,
                customPaint = x.customPaint,
                image = x.image,
                isImage = x.isImage,
                isNumber = x.isNumber,
                RowId = x.RowId,
                useCustomFont = x.useCustomFont,
                Value = x.Value,
                backgroundColorPaint = x.backgroundColorPaint,
                hasBackgroundColor = x.hasBackgroundColor,
            }).ToList();
            pdfBulderClass.Doc = CreatePdf(specs, header.ToList(), footer.ToList(), dataPDF.Select(x => x).ToList());
            pdfBulderClass.Attachments = CreateImages(specs, header.ToList(), footer.ToList(), dataImages.Select(x => x).ToList());
            return pdfBulderClass;
        }

        private List<byte[]> CreateImages(ReportDrawSpecs specs, List<ReportDrawDetails> header, List<ReportDrawDetails> footer, List<ReportDrawDetails> data)
        {
            specs.ColWidth = specs.ColWidth.Select(x => (x - gridSpacing)).ToList();
            return CrteImages(specs, header, footer, data);
        }
        private byte[] CreatePdf(ReportDrawSpecs specs, List<ReportDrawDetails> header, List<ReportDrawDetails> footer, List<ReportDrawDetails> data)
        {
            specs.ColWidth = specs.ColWidth.Select(x => (x - gridSpacing)).ToList();
            return CrtePdf(specs, header, footer, data);
        }
        private List<byte[]> CrteImages(ReportDrawSpecs specs, List<ReportDrawDetails> header, List<ReportDrawDetails> footer, List<ReportDrawDetails> data)
        {
            var attachments = new List<byte[]>();

            var unwrittenData = new List<ReportDrawDetails>(data.OrderBy(x => x.RowId).ThenBy(x => x.ColNo).Select(x => x).ToList());
            bool isfinished = false;
            int pageIndex = 0;
            using (var surface = SKSurface.Create(new SKImageInfo(pdfWidth, pdfHeight)))
            {
                while (isfinished != true)
                {
                    using (var pdfCanvas = surface.Canvas)
                    {
                        float yup = 0;
                        yup = GenerateHeader(pdfCanvas, yup, specs, header);
                        var ydown = GenerateFooter(pdfCanvas, pdfHeight, yup, pageIndex, specs, footer, unwrittenData);
                        yup = GenerateBody(pdfCanvas, ydown, yup, specs, unwrittenData);
                        ResetDataRowIndex(unwrittenData);
                        pageIndex = pageIndex + 1;
                        if (unwrittenData.Count <= 0)
                        {
                            isfinished = true;
                        }
                    }
                    attachments.Add(surface.Snapshot().Encode(SkiaSharp.SKEncodedImageFormat.Png, 10).ToArray());
                }
            }
            return attachments;
        }
        private byte[] CrtePdf(ReportDrawSpecs specs, List<ReportDrawDetails> header, List<ReportDrawDetails> footer, List<ReportDrawDetails> data)
        {
            var pdfStream = new MemoryStream();
            var unwrittenData = new List<ReportDrawDetails>(data.OrderBy(x => x.RowId).ThenBy(x => x.ColNo).Select(x => x).ToList());
            bool isfinished = false;
            int pageIndex = 0;
            using (var document = SKDocument.CreatePdf(pdfStream, new SKDocumentPdfMetadata
            {
                Creation = DateTime.Now,
                Creator = "FinanceApps",
                PdfA = true
            }))
            {
                while (isfinished != true)
                {
                    using (var pdfCanvas = document.BeginPage(pdfWidth, pdfHeight))
                    {
                        float yup = 0;
                        yup = GenerateHeader(pdfCanvas, yup, specs, header);
                        var ydown = GenerateFooter(pdfCanvas, pdfHeight, yup, pageIndex, specs, footer, unwrittenData);
                        yup = GenerateBody(pdfCanvas, ydown, yup, specs, unwrittenData);
                        ResetDataRowIndex(unwrittenData);
                        pageIndex = pageIndex + 1;
                        if (unwrittenData.Count <= 0)
                        {
                            isfinished = true;
                        }
                        document.EndPage();

                    }
                }
                document.Close();
            }
            return pdfStream.ToArray();
        }

        private float GenerateHeader(SKCanvas pdfCanvas, float yup, ReportDrawSpecs specs, List<ReportDrawDetails> header)
        {
            pdfCanvas.Clear(skWhite);
            // write header
            foreach (var item in header.OrderBy(x => x.RowId).ThenBy(x => x.ColNo))
            {
                var x = Padxleft + 5 + specs.ColWidth.Take(item.ColNo > 1 ? item.ColNo - 1 : 0).Sum() + ((item.ColNo > 1 ? item.ColNo - 1 : 0) * gridSpacing);
                var y = Padyup + ((rowheight + gridSpacing) * (item.RowId));

                GenerateStyling(pdfCanvas, item, specs, x, y);
                if (item.isNumber)
                {
                    x = (Padxleft - 5 + specs.ColWidth.Take(item.ColNo).Sum());
                }

                var width = (specs.ColWidth.Skip(item.ColNo - 1).Take(item.colSpan).Sum()) + (gridSpacing * 2) + ((item.colSpan - 1) * gridSpacing);
                var paint = ChoosePaint(item);
                var textX = x;
                if (paint.TextAlign == SKTextAlign.Center)
                {
                    textX = x - 5 + (width / 2);
                }

                var wordWithSpaceLength = (item.isNumber == true ? pntDataGridBodyRight : pntDataGridBodyLeft).MeasureText(item.Value);
                var newvalue = wordWithSpaceLength > width ? item.Value.Substring(0, (int)Math.Floor(width / (wordWithSpaceLength / item.Value.Length))) : item.Value;
                if (item.isImage)
                {
                    var a = GetImage(item.image, int.Parse(item.Value));
                    pdfCanvas.DrawImage(a, textX, y);
                }
                else
                {
                    pdfCanvas.DrawText(newvalue, textX, y, ChoosePaint(item));
                }
                yup = yup > y ? yup : y;
            }
            return yup;
        }
        private float GenerateFooter(SKCanvas pdfCanvas, float ydown, float yup, int pageIndex, ReportDrawSpecs specs, List<ReportDrawDetails> footer, List<ReportDrawDetails> unwrittenData)
        {
            // write footer

            foreach (var item in footer.OrderBy(x => x.RowId).ThenBy(x => x.ColNo))
            {
                var x = (Padxleft + 5 + specs.ColWidth.Take(item.ColNo > 1 ? item.ColNo - 1 : 0).Sum() + ((item.ColNo > 1 ? item.ColNo - 1 : 0) * gridSpacing));
                var y = Padydown - ((rowheight + gridSpacing) * (item.RowId));

                GenerateStyling(pdfCanvas, item, specs, x, y);
                if (item.isNumber)
                {
                    x = (Padxleft - 5 + specs.ColWidth.Take(item.ColNo).Sum());
                }
                var wordWithSpaceLength = (item.isNumber == true ? pntDataGridBodyRight : pntDataGridBodyLeft).MeasureText(item.Value);
                var colWidth = (specs.ColWidth.Skip(item.ColNo - 1).Take(item.colSpan).Sum()) + (gridSpacing * 2) + ((item.colSpan - 1) * gridSpacing);
                var newvalue = wordWithSpaceLength > colWidth ? item.Value.Substring(0, (int)Math.Floor(colWidth / (wordWithSpaceLength / item.Value.Length))) : item.Value;
                if (item.isImage)
                {
                    var a = GetImage(item.image, int.Parse(item.Value));
                    pdfCanvas.DrawImage(a, x, y);
                }
                else
                {
                    pdfCanvas.DrawText(newvalue, x, y, ChoosePaint(item));
                }
                ydown = ydown < y ? ydown : y;
            }

            var rowHelper = (int)Math.Floor((ydown - yup) / rowheight);
            var pages = ((int)Math.Ceiling((decimal)(unwrittenData.GroupBy(x => x.RowId).Select(x => x.Key).ToList().Count / rowHelper)));
            totalPages = pages >= totalPages ? pages : totalPages;
            if (specs.HasPageNum)
            {
                var x = (Padxleft + 5 + specs.ColWidth.Take(specs.pageNum.ColNo > 1 ? specs.pageNum.ColNo - 1 : 0).Sum() + ((specs.pageNum.ColNo > 1 ? specs.pageNum.ColNo - 1 : 0) * gridSpacing));
                var y = Padydown - ((rowheight + gridSpacing) * (specs.pageNum.RowId));

                GenerateStyling(pdfCanvas, specs.pageNum, specs, x, y);
                if (specs.pageNum.isNumber)
                {
                    x = (Padxleft - 5 + specs.ColWidth.Take(specs.pageNum.ColNo).Sum());
                }
                specs.pageNum.Value = $"Page {pageIndex + 1} of {totalPages + 1}";
                pdfCanvas.DrawText(specs.pageNum.Value, x, y, ChoosePaint(specs.pageNum));
                ydown = ydown < y ? ydown : y;
            }
            return ydown;
        }
        private float GenerateBody(SKCanvas pdfCanvas, float ydown, float yup, ReportDrawSpecs specs, List<ReportDrawDetails> unwrittenData)
        {
            // write data
            var ysave = yup;
            foreach (var item in unwrittenData.OrderBy(x => x.RowId).ThenBy(x => x.ColNo))
            {
                var x = Padxleft + 5 + specs.ColWidth.Take(item.ColNo > 1 ? item.ColNo - 1 : 0).Sum() + ((item.ColNo > 1 ? item.ColNo - 1 : 0) * gridSpacing);
                var y = ysave + ((rowheight + gridSpacing) * ((item.RowId)));
                if (y < (ydown - rowheight))
                {
                    GenerateStyling(pdfCanvas, item, specs, x, y);
                    if (item.isNumber)
                    {
                        x = (Padxleft - 5 + specs.ColWidth.Take(item.ColNo).Sum());
                    }

                    var wordWithSpaceLength = (item.isNumber == true ? pntDataGridBodyRight : pntDataGridBodyLeft).MeasureText(item.Value);
                    var colWidth = (specs.ColWidth.Skip(item.ColNo - 1).Take(item.colSpan).Sum()) + (gridSpacing * 2) + ((item.colSpan - 1) * gridSpacing);
                    var newvalue = wordWithSpaceLength > colWidth ? item.Value.Substring(0, (int)Math.Floor(colWidth / (wordWithSpaceLength / item.Value.Length))) : item.Value;
                    if (item.isImage)
                    {
                        var a = GetImage(item.image, int.Parse(item.Value));
                        pdfCanvas.DrawImage(a, x, y);
                    }
                    else
                    {
                        pdfCanvas.DrawText(newvalue, x, y, ChoosePaint(item));
                    }
                    yup = yup > y ? yup : y;

                    unwrittenData.Remove(item);
                }
                else
                {

                }
            }
            return yup;
        }
        private void GenerateStyling(SKCanvas pdfCanvas, ReportDrawDetails item, ReportDrawSpecs specs, float x, float y)
        {
            float newX = x - 5 - gridSpacing;
            float newY = y - rowheight + 5 - gridSpacing;
            float newW = (specs.ColWidth.Skip(item.ColNo - 1).Take(item.colSpan).Sum()) + ((item.colSpan + 1) * gridSpacing);
            float newH = rowheight + (2 * gridSpacing);
            SKPoint topLeft = new SKPoint(newX, newY);
            SKPoint topRight = new SKPoint(newX + newW, newY);
            SKPoint botLeft = new SKPoint(newX, newY + newH);
            SKPoint botRight = new SKPoint(newX + newW, newY + newH);
            if (item.hasBackgroundColor)
            {
                pdfCanvas.DrawRect(newX, newY, newW, newH, ConvertMakePaint(item.backgroundColorPaint));
            }
            if (item.borderTop)
            {
                pdfCanvas.DrawLine(topLeft, topRight, pntGridLines);
            }
            if (item.borderBottom)
            {
                pdfCanvas.DrawLine(botLeft, botRight, pntGridLines);
            }
            if (item.borderLeft)
            {
                pdfCanvas.DrawLine(topLeft, botLeft, pntGridLines);
            }
            if (item.borderRight)
            {
                pdfCanvas.DrawLine(topRight, botRight, pntGridLines);
            }
        }
        private void ResetDataRowIndex(List<ReportDrawDetails> unwrittenData)
        {
            int index = 1;
            foreach (var row in unwrittenData.GroupBy(x => x.RowId).Select(x => x.Key))
            {
                unwrittenData.Where(x => x.RowId == row).ToList().ForEach(x => x.RowId = index);
                index++;
            }
        }

        private SKPaint ChoosePaint(ReportDrawDetails item)
        {
            if (item.useCustomFont)
            {
                return ConvertMakePaint(item.customPaint);
            }
            else if (item.isNumber)
            {
                return pntDataGridBodyRight;
            }
            else if (item.Centre)
            {
                return MakeSKPaint(skBlack, true, 11, SKTextAlign.Center);
            }
            else
            {
                return pntDataGridBodyLeft;
            }
        }


        public SKPaint ConvertMakePaint(MakePaint makePaint)
        {
            if (makePaint.HexColor.StartsWith("#"))
                makePaint.HexColor = makePaint.HexColor.Substring(1); // Remove the '#' if present

            if (makePaint.HexColor.Length != 6)
                throw new ArgumentException("Invalid hex code. Hex code must be 6 characters long.");

            // Parse the RGB components from the hex code
            int red = int.Parse(makePaint.HexColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int green = int.Parse(makePaint.HexColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int blue = int.Parse(makePaint.HexColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            var skColor = new SKColor((byte)red, (byte)green, (byte)blue);
            return MakeSKPaint(skColor, makePaint.IsAntiAlias, makePaint.TextSize, (SKTextAlign)makePaint.Align, makePaint.IsBold);
        }
        public SKImage GetImage(byte[] image, int size = 80)
        {
            var logo = SKBitmap.Decode(image);
            if (logo.Height >= size)
            {
                var newH = logo.Height - (logo.Height - size);
                var newW = (logo.Width / logo.Height) * newH;
                logo = logo.Resize(new SKImageInfo(newW, newH), SKFilterQuality.High);
            }
            else
            {
                var newH = logo.Height - (logo.Height - size);
                var newW = (logo.Width / logo.Height) * newH;
                logo = logo.Resize(new SKImageInfo(newW, newH), SKFilterQuality.High);
            }
            return SKImage.FromBitmap(logo);
        }

        public SKPaint MakeSKPaint(SKColor color, bool isAntiAlias, float textSize, SKTextAlign align, bool isBold = false)
        {
            SKPaint paint = new SKPaint();
            paint.Color = color;
            paint.IsAntialias = isAntiAlias;
            paint.TextSize = textSize;
            paint.TextAlign = align;
            if (isBold)
            {
                paint.Typeface = SKTypeface.FromFamilyName(
                "Helvetica", SKFontStyleWeight.Medium, SKFontStyleWidth.Condensed, SKFontStyleSlant.Upright);
            }
            else
            {
                paint.Typeface = SKTypeface.FromFamilyName(
                "Helvetica");
            }
            return paint;
        }
    }
}

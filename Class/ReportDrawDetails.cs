namespace DocBuilder.Class;

public class ReportDrawDetails
{
    public string Value { get; set; }
    public int ColNo { get; set; }
    public bool isNumber { get; set; }
    public int RowId { get; set; }
    public int colSpan { get; set; }
    public bool isImage { get; set; }
    public byte[] image { get; set; }
    public bool Centre { get; set; }
    public bool useCustomFont { get; set; }
    public MakePaint customPaint { get; set; }
    public bool hasBackgroundColor { get; set; }
    public MakePaint backgroundColorPaint { get; set; }
    public bool borderRight { get; set; }
    public bool borderLeft { get; set; }
    public bool borderTop { get; set; }
    public bool borderBottom { get; set; }
}
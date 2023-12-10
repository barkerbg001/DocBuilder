using DocBuilder.Enums;
namespace DocBuilder.Class;

public class MakePaint
{
    public string HexColor { get; set; }
    public bool IsAntiAlias { get; set; }
    public float TextSize { get; set; }
    public Align Align { get; set; }
    public bool IsBold { get; set; }
}
using DocBuilder.Enums;

namespace DocBuilder.Class;
public class ReportDrawSpecs
{
    public List<int> ColWidth { get; set; }
    public bool HasPageNum { get; set; }
    public Orientation Orientation { get; set; }
    public ReportDrawDetails pageNum { get; set; }
}

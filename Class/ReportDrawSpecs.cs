using DocBuilder.Enums;

namespace DocBuilder.Class;
public class ReportDrawSpecs
{    
    public DateTime Creation { get; set; }
    public string Creator { get; set; }
    public bool PdfA { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public string Subject { get; set; }
    public string Keywords { get; set; }
    public string Producer { get; set; }

    public List<int> ColWidth { get; set; }
    public bool HasPageNum { get; set; }
    public Orientation Orientation { get; set; }
    public ReportDrawDetails pageNum { get; set; }
}

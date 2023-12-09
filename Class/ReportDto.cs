namespace DocBuilder.Class;

public class ReportDto
{
    public ReportDrawSpecs specs { get; set; }
    public List<ReportDrawDetails> header { get; set; }
    public List<ReportDrawDetails> footer { get; set; }
    public List<ReportDrawDetails> data { get; set; }
}
namespace Clock.Maui.Model;

public class ReportOptions
{
    public bool IncludeBullets { get; set; }
    public string[] IncludeFields { get; set; }
    public bool QuoteFieldsWithSpaces { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
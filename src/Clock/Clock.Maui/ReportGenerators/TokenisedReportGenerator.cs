using System.Text;
using Clock.Maui.Model;

namespace Clock.Maui.ReportGenerators;

public class TokenisedReportGenerator : IReportGenerator
{
    private readonly char _token;
    internal char Token
    {
        get => _token;
    }

    public TokenisedReportGenerator(char token)
    {
        _token = token;
    }


    public virtual string GenerateReport(IEnumerable<WorkItem> workItems, ReportOptions reportOptions)
    {
        StringBuilder sb = new StringBuilder();
        foreach (WorkItem workItem in workItems)
        {
            sb.AppendLine(BuildCsvLineFromWorkItem(workItem));
        }
        return sb.ToString();
    }

    private string BuildCsvLineFromWorkItem(WorkItem workItem)
    {
        StringBuilder sb=new StringBuilder();

        sb.Append($"{workItem.StartTime}{Token}{workItem.RecordedTime}{Token}{workItem.Title}");
        
        return sb.ToString();
    }

    
}
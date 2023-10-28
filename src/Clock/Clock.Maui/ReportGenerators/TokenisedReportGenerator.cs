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
            sb.AppendLine(BuildLineFromWorkItem(workItem,reportOptions));
        }
        return sb.ToString();
    }

    private string BuildLineFromWorkItem(WorkItem workItem, ReportOptions reportOptions)
    {
        List<string> fieldValues = new List<string>();
        
        if (reportOptions.IncludeFields.Contains("StartTime")) fieldValues.Add(workItem.StartTime.ToString("g"));
        if (reportOptions.IncludeFields.Contains("StopTime")) fieldValues.Add(workItem.StopTime.ToString("g"));
        if (reportOptions.IncludeFields.Contains("RecordedTime")) fieldValues.Add(workItem.RecordedTime.ToString("c"));
        if (reportOptions.IncludeFields.Contains("Title")) fieldValues.Add(reportOptions.QuoteFieldsWithSpaces ? $"\"{workItem.Title}\"" : workItem.Title);
        
        StringBuilder sb=new StringBuilder();
        sb.Append(string.Join(_token, fieldValues));

        if (reportOptions.IncludeBullets) sb.Insert(0, "* ");
        return sb.ToString();
    }

    
}
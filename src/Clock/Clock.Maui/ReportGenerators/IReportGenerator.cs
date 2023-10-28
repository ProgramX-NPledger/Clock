using Clock.Maui.Model;

namespace Clock.Maui.ReportGenerators;

public interface IReportGenerator
{
    string GenerateReport(IEnumerable<WorkItem> workItems, ReportOptions reportOptions);
}
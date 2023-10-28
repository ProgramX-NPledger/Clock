using Clock.Maui.Model;
using Clock.Maui.ViewModel;

namespace Clock.Maui.ReportGenerators;

public static class ReportGeneratorFactory
{
    public static IReportGenerator CreateReportGeneratorForFormat(ReportFormat reportFormat)
    {
        switch (reportFormat)
        {
            case ReportFormat.Csv:
                return new TokenisedReportGenerator(',');
            case ReportFormat.Tsv:
                return new TokenisedReportGenerator('\t');
            default:
                throw new NotSupportedException($"ReportFormat {reportFormat} unsupported");
        }
    }
}
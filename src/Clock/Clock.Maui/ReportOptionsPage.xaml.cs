using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clock.Maui.Data;
using Clock.Maui.Model;
using Clock.Maui.ReportGenerators;
using Clock.Maui.ViewModel;

namespace Clock.Maui;

public partial class ReportOptionsPage : ContentPage
{
	public ReportOptionsPage()
	{
		InitializeComponent();
		//NavigationPage.SetHasNavigationBar(this,true);
		ReportOptionsViewModel reportOptionsViewModel = (ReportOptionsViewModel) BindingContext;
		reportOptionsViewModel.ReportRequired += async (s, e) =>
		{
			// get the report details
			IEnumerable<WorkItem> workItems=await App.WorkItemRepository.GetWorkItems(new WorkItemCriteria()
			{

			});
			
			// get a report generator
			IReportGenerator reportGenerator = ReportGeneratorFactory.CreateReportGeneratorForFormat(e.ReportFormat);

			string reportText = reportGenerator.GenerateReport(workItems, e.Options);
			// load the report viewer and populate with report
			ReportViewerPage reportViewerPage = new ReportViewerPage();
			((ReportViewerPageViewModel)reportViewerPage.BindingContext).ReportText = reportText;
			// Window reportDialogWindow = new Window(reportPage);
			// Application.Current?.OpenWindow(reportDialogWindow);
			// reportDialogWindow.Navigation.PushModalAsync(reportPage);
			Navigation.PushAsync(reportViewerPage); // using Modal removes Back button

		};
	}
}
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

public partial class ReportPage : ContentPage
{
	public ReportPage()
	{
		InitializeComponent();
		//NavigationPage.SetHasNavigationBar(this,true);
		ReportViewModel reportViewModel = (ReportViewModel) BindingContext;
		reportViewModel.ReportRequired += async (s, e) =>
		{
			// get the report details
			IEnumerable<WorkItem> workItems=await App.WorkItemRepository.GetWorkItems(new WorkItemCriteria()
			{

			});
			
			// get a report generator
			IReportGenerator reportGenerator = ReportGeneratorFactory.CreateReportGeneratorForFormat(e.ReportFormat);
			
			// load the report viewer and populate with report
			// generate the report - maybe on a different page
			
		};
	}
}
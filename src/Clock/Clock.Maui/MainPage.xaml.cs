using System.Collections.ObjectModel;
using Clock.Maui.Data;
using Clock.Maui.Model;
using Clock.Maui.ViewModel;
using SQLite;

namespace Clock.Maui;

public partial class MainPage : ContentPage
{
    private IDispatcherTimer _mainClock;

    
    public MainPage()
    {
        InitializeComponent();
        //BindingContext = new MainViewModel(); // could use this to use a non-default ctor
        
        _mainClock = CreateMainClock();
        MainViewModel viewModel = (MainViewModel)BindingContext;
        viewModel.RequestMainTimerStart += (s, e) =>
        {
            _mainClock.Start();
        };
        viewModel.RequestMainTimerStop += (s, e) =>
        {
            _mainClock.Stop();
        };
        viewModel.RequestLoadLatestPersistedWorkItems += async (s, e) =>
        {
            viewModel.WorkItems = GroupByDate(await App.WorkItemRepository.GetLastNWorkItems(5));
        };
        viewModel.WorkItemAdded += async (s, e) =>
        {
            await App.WorkItemRepository.AddCurrentWorkItemToDatabase(e.WorkItem);
        };
        viewModel.RequestOpenReportDialog+=(s,e) =>
            {
                ReportPage reportPage = new ReportPage();
                // Window reportDialogWindow = new Window(reportPage);
                // Application.Current?.OpenWindow(reportDialogWindow);
                // reportDialogWindow.Navigation.PushModalAsync(reportPage);
                Navigation.PushModalAsync(reportPage);
                //
            }
        ;
    }

    private ObservableCollection<WorkItemGroupByDate> GroupByDate(IEnumerable<WorkItem> workItems)
    {
        IOrderedEnumerable<WorkItem> sortedWorkItems = workItems.OrderByDescending(o => o.StartTime);
       
        ObservableCollection<WorkItemGroupByDate>
            workItemsGroupedByDate = new ObservableCollection<WorkItemGroupByDate>();
        
        foreach (WorkItem workItem in sortedWorkItems)
        {
            DateTime date = workItem.StartTime.Date;
            WorkItemGroupByDate workItemGroupByDate = workItemsGroupedByDate.SingleOrDefault(q => q.Date == date);
            if (workItemGroupByDate == null)
            {
                workItemGroupByDate = new WorkItemGroupByDate(GetFriendlyNameForDate(date), date,
                    new ObservableCollection<WorkItem>());
                workItemsGroupedByDate.Add(workItemGroupByDate);
            }
            workItemGroupByDate.Add(workItem);
        }

        return workItemsGroupedByDate;
    }

    private string GetFriendlyNameForDate(DateTime date)
    {
        if (DateTime.Now.Date == date.Date) return "Today";
        if (DateTime.Now.AddDays(-1).Date == date) return "Yesterday";
        if (DateTime.Now.Date.AddDays(-7) > date.Date)
            return $"{(DateTime.Now.Date - date).Days} days ago";
        return date.ToLongDateString();
    }


    private IDispatcherTimer CreateMainClock()
    {
        if (Application.Current != null)
        {
            IDispatcherTimer dispatcherTimer =Application.Current.Dispatcher.CreateTimer();
			
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += (sender, args) =>
            {
                var viewModel = this.BindingContext;
                ((MainViewModel)viewModel).MainTimerValue = DateTime.Now - ((MainViewModel)viewModel).MainTimerLastStartedAt;
//				 ToString("h\\:mm\\:ss");
            };

            return dispatcherTimer;

        }
        throw new InvalidOperationException("Failed to create Main Clock because Application.Current is null");
    }
    

 

}
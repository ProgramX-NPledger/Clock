using System.Collections.ObjectModel;
using Clock.Maui.Data;
using Clock.Maui.Model;
using Clock.Maui.Services;
using Clock.Maui.ViewModel;
using SQLite;
using Preferences = Xamarin.Essentials.Preferences;

namespace Clock.Maui;

public partial class MainPage : ContentPage
{
    private IDispatcherTimer _mainClock;

    
    public MainPage()
    {
        InitializeComponent();
        
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
        viewModel.RequestOpenReportDialog+= (s, e) =>
        {
            ReportOptionsPage reportPage = new ReportOptionsPage();
            Navigation.PushAsync(reportPage); // using Modal removes Back button
        };
        viewModel.UpdateAvailable += async (s, e) =>
        {
            // pop up confirmation for user
            string message = $"An upgrade to {e.AvailableUpdateStatus.AvailableRelease.Version} " +
                             $"(your current version is {e.AvailableUpdateStatus.CurrentVersion}) " +
                             $"is available.{Environment.NewLine}{Environment.NewLine}" +
                             $"Do you want to download and prepare the upgrade now?";
            if (e.AvailableUpdateStatus.AvailableRelease.IsPreRelease)
            {
                message +=
                    $"{Environment.NewLine}{Environment.NewLine}This upgrade is a pre-release upgrade and may not " +
                    "be stable. This preference can be changed in settings.";
            }
            bool confirmUpgrade = await DisplayAlert("Update available", message, "Upgrade", "No");
            if (confirmUpgrade)
            {
                ((MainViewModel) BindingContext).StatusText = "Downloading and preparing update ...";
                using (GitHubUpdateService gitHubUpdateService = new GitHubUpdateService())
                {
                    byte[] updateFile=await gitHubUpdateService.DownloadAndPrepareUpdate(e.AvailableUpdateStatus);
                    ((MainViewModel) BindingContext).StatusText = "Successfully downloaded";
                    // TODO: we'll temporarily save the file and get the user to update
                    // https://devblogs.microsoft.com/dotnet/file-and-folder-dialogs-communitytoolkit/
                }                
            }
            
        };

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
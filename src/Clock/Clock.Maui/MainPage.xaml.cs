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
            viewModel.WorkItems = new ObservableCollection<WorkItem>(await App.WorkItemRepository.GetLastNWorkItems(5));
        };
        viewModel.WorkItemAdded += async (s, e) =>
        {
            await App.WorkItemRepository.AddCurrentWorkItemToDatabase(e.WorkItem);
        };

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
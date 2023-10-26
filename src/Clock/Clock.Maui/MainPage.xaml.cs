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
        ((MainViewModel)BindingContext).RequestMainTimerStart += (s, e) =>
        {
            _mainClock.Start();
        };
        ((MainViewModel)BindingContext).RequestMainTimerStop += (s, e) =>
        {
            _mainClock.Stop();
        };
        ((MainViewModel)BindingContext).RequestLoadLatestPersistedWorkItems += async (s, e) =>
        {
            ((MainViewModel)BindingContext).WorkItems = await App.WorkItemRepository.GetLastNWorkItems(5);
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
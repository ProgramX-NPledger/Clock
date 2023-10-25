using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
//using Android.Locations;
using Clock.Maui.Model;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Clock.Maui.ViewModel;

public class MainViewModel : INotifyPropertyChanged
{
	private DateTime _mainTimerLastStartedAt;
	private TimeSpan _mainTimerValue;
	private string _workItemEntry;
	private string _mainTimerButtonText;
	private bool _isMainTimerRunning;

	public event EventHandler<EventArgs> RequestMainTimerStart;
	public event EventHandler<EventArgs> RequestMainTimerStop;
	
	public bool IsMainTimerRunning
	{
		get => _isMainTimerRunning;
		set
		{
			if (_isMainTimerRunning != value)
			{
				_isMainTimerRunning = value;
				OnPropertyChanged();
			}
		}
	}
	

	public DateTime MainTimerLastStartedAt
	{
		get => _mainTimerLastStartedAt;
		set
		{
			if (_mainTimerLastStartedAt != value)
			{
				_mainTimerLastStartedAt = value;
				OnPropertyChanged();
			}
		}
	}
	
	public string MainTimerButtonText
	{
		get => _mainTimerButtonText;
		set
		{
			if (_mainTimerButtonText != value)
			{
				_mainTimerButtonText = value;
				OnPropertyChanged();
			}
		}
	}
	
	public TimeSpan MainTimerValue
	{
		get => _mainTimerValue;
		set
		{
			if (_mainTimerValue != value)
			{
				_mainTimerValue = value;
				OnPropertyChanged();
			}
		}
	}

	public string WorkItemEntry
	{
		get => _workItemEntry;
		set
		{
			if (_workItemEntry != value)
			{
				_workItemEntry = value;
				OnPropertyChanged();
			}
		}
	}
	
	public ICommand StartStopButtonCommand { get; private set; }

	
	
	
	// MVVM requires a default constructor
	public MainViewModel()
	{
		StartStopButtonCommand = new Command(() =>
		{
			StopOrStartMainClock();
		});
	}



	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	protected virtual void OnRequestMainTimerStart(EventArgs e)
	{
		if (RequestMainTimerStart != null) RequestMainTimerStart(this,e);
	}

	protected virtual void OnRequestMainTimerStop(EventArgs e)
	{
		if (RequestMainTimerStop != null) RequestMainTimerStop(this,e);
	}
	
	// protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
	// {
	// 	if (EqualityComparer<T>.Default.Equals(field, value)) return false;
	// 	field = value;
	// 	OnPropertyChanged(propertyName);
	// 	return true;
	// }
	
  

    private void StopOrStartMainClock()
    {
        if (IsMainTimerRunning)
        {
            StopMainClock();
            AddCurrentWorkItemToDatabase();
        }
        else
        {
            StartMainClock();
        }
    }

    private void AddCurrentWorkItemToDatabase()
    {
        App.WorkItemRepository.AddCurrentWorkItemToDatabase(new WorkItem()
        {
            Title = WorkItemEntry,
            RecordedTime = MainTimerValue,
            StartTime = _mainTimerLastStartedAt,
            StopTime = DateTime.Now
        });
    }

    private void WorkItemEntry_OnCompleted(object sender, EventArgs e)
    {
        // user has pressed Enter on text box, so start the clock
        Entry entry = (Entry)sender;

        // if the clock is not running, start it
        if (IsMainTimerRunning)
        {
            StartMainClock();
        }
        else
        {
            // if it's already running, we should add the current work item, stop the clock and restart it with the new WorkItem
            
        }

    }

    private void StartMainClock()
    {
        MainTimerButtonText = "Stop";
        
        MainTimerLastStartedAt = DateTime.Now;
        IsMainTimerRunning = true;
        OnRequestMainTimerStart(EventArgs.Empty);
    }

    private void StopMainClock()
    {
		OnRequestMainTimerStop(EventArgs.Empty);
        WorkItemEntry = string.Empty;
        IsMainTimerRunning = false;
        MainTimerButtonText = "Start";
    }

  
	
}
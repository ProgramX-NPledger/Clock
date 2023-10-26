using System.Collections.ObjectModel;
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
	private ObservableCollection<WorkItem> _workItems;
	
	public event EventHandler<EventArgs> RequestMainTimerStart;
	public event EventHandler<EventArgs> RequestMainTimerStop;
	public event EventHandler<EventArgs> RequestLoadLatestPersistedWorkItems;
	public event PropertyChangedEventHandler PropertyChanged;
	public event EventHandler<WorkItemEventArgs> WorkItemAdded;
	

	public ObservableCollection<WorkItem> WorkItems
	{
		get => _workItems;
		set
		{
			if (_workItems != value)
			{
				_workItems = value;
				OnPropertyChanged();
			}
		}
	}
	
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
	public ICommand LoadLatestPersistedWorkItemsCommand { get; private set; }
	
	
	
	// MVVM requires a default constructor
	public MainViewModel()
	{
		StartStopButtonCommand = new Command(() =>
		{
			StopOrStartMainClock();
		});
		LoadLatestPersistedWorkItemsCommand = new Command(() =>
		{
			OnRequestLoadLatestPersistedWorkItems();
		});
	}




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
	
	protected virtual void OnRequestLoadLatestPersistedWorkItems()
	{
		RequestLoadLatestPersistedWorkItems?.Invoke(this, EventArgs.Empty);
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
	    WorkItem workItem = new WorkItem()
	    {
		    Title = WorkItemEntry,
		    RecordedTime = MainTimerValue,
		    StartTime = _mainTimerLastStartedAt,
		    StopTime = DateTime.Now
	    };
	    
	    WorkItems.Add(workItem);
	    OnWorkItemAdded(new WorkItemEventArgs()
	    {
		    WorkItem = workItem
	    });
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


    protected virtual void OnWorkItemAdded(WorkItemEventArgs e)
    {
	    WorkItemAdded?.Invoke(this, e);
    }
}
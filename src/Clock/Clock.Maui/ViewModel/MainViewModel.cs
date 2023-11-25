using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Clock.Maui.Commands;
using Clock.Maui.EventArgs;
//using Android.Locations;
using Clock.Maui.Model;
using Clock.Maui.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Clock.Maui.ViewModel;

public class MainViewModel : INotifyPropertyChanged
{
	private DateTime _mainTimerLastStartedAt;
	private TimeSpan _mainTimerValue;
	private string _workItemEntry;
	private string _mainTimerButtonText;
	private bool _isMainTimerRunning;
	private ObservableCollection<WorkItemGroupByDate> _workItems;

	public event EventHandler<UpdateAvailableEventArgs> UpdateAvailable;
	public event EventHandler<System.EventArgs> RequestMainTimerStart;
	public event EventHandler<System.EventArgs> RequestMainTimerStop;
	public event EventHandler<System.EventArgs> RequestLoadLatestPersistedWorkItems;
	public event PropertyChangedEventHandler PropertyChanged;
	public event EventHandler<WorkItemEventArgs> WorkItemAdded;

	public event EventHandler<System.EventArgs> RequestOpenReportDialog; 

	public ObservableCollection<WorkItemGroupByDate> WorkItems
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

	public ICommand DisplayReportDialogCommand { get; private set; }
	
	public IAsyncCommand CheckForUpdateCommand { get; private set; }


	// MVVM requires a default constructor
	public MainViewModel()
	{
		StartStopButtonCommand = new Command(() => { StopOrStartMainClock(); });
		LoadLatestPersistedWorkItemsCommand = new Command(() => { OnRequestLoadLatestPersistedWorkItems(); });
		DisplayReportDialogCommand = new Command(() => { OnRequestOpenReportDialog(); });
		CheckForUpdateCommand = new AsyncCommand( async () => // TODO avoid using async within ctor https://stackoverflow.com/questions/54232156/how-to-implement-async-comman
		{
			bool preferPreRelease = Preferences.Get(GitHubUpdateService.PREFER_PRERELEASE_CONFIG_STRING, false);
			preferPreRelease = true; // TODO: remove when preferences can be saved
			using (GitHubUpdateService gitHubUpdateService = new GitHubUpdateService())
			{
			    AvailableUpdateStatus latestUpdate = await gitHubUpdateService.GetUpdateStatus(preferPreRelease);
			    if (latestUpdate.IsUpdateAvailable())
			    {
				    // fire an event so the UI thread can pick up the interaction with the user
				    UpdateAvailable?.Invoke(this,new UpdateAvailableEventArgs()
				    {
					    AvailableUpdateStatus = latestUpdate
				    });
			    }
			    

			    
			}

		});
	}




	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	protected virtual void OnRequestMainTimerStart(System.EventArgs e)
	{
		if (RequestMainTimerStart != null) RequestMainTimerStart(this, e);
	}

	protected virtual void OnRequestMainTimerStop(System.EventArgs e)
	{
		if (RequestMainTimerStop != null) RequestMainTimerStop(this, e);
	}

	protected virtual void OnRequestLoadLatestPersistedWorkItems()
	{
		RequestLoadLatestPersistedWorkItems?.Invoke(this, System.EventArgs.Empty);
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
			
			WorkItemEntry = string.Empty;
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

		WorkItemGroupByDate todaysWorkItemsGroup = WorkItems.SingleOrDefault(q => q.Date == DateTime.Now.Date);
		if (todaysWorkItemsGroup == null)
		{
			// today isn't in the list yet, so create it and add it
			WorkItems.Insert(0,
				new WorkItemGroupByDate("Today",
					DateTime.Now.Date,
					new ObservableCollection<WorkItem>(
						new WorkItem[]
						{
							workItem
						}
					)
				)
			);
		}
		else
		{
			todaysWorkItemsGroup.Insert(0, workItem);
		}

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
		OnRequestMainTimerStart(System.EventArgs.Empty);
	}

	private void StopMainClock()
	{
		OnRequestMainTimerStop(System.EventArgs.Empty);
		IsMainTimerRunning = false;
		MainTimerButtonText = "Start";
	}


	protected virtual void OnWorkItemAdded(WorkItemEventArgs e)
	{
		WorkItemAdded?.Invoke(this, e);
	}

	protected virtual void OnRequestOpenReportDialog()
	{
		RequestOpenReportDialog?.Invoke(this, System.EventArgs.Empty);
	}
}

using Clock.Maui.Model;
using SQLite;

namespace Clock.Maui;

public partial class MainPage : ContentPage
{
    private IDispatcherTimer _mainClock;
    private DateTime _mainClockLastStartedAt;
    
    public MainPage()
    {
        InitializeComponent();
    }

    private SQLiteConnection GetSqLiteConnection()
    {
        string fileName = Path.Combine(FileSystem.AppDataDirectory, "workitems.sqlite.db");
        SQLiteConnection sqLiteConnection = new SQLiteConnection(fileName);
        CreateTableResult createTableResult=sqLiteConnection.CreateTable<WorkItem>();
        return sqLiteConnection;
    }


    private void StartStopButton_OnClicked(object sender, EventArgs e)
    {
        StopOrStartMainClock();
    }

    private void StopOrStartMainClock()
    {
        if (_mainClock is { IsRunning: true })
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
        SQLiteConnection sqLiteConnection = GetSqLiteConnection();
        WorkItem workItem = new WorkItem()
        {
            Title = WorkItemEntry.Text,
            RecordedTime = CalculateMainTimerDifference(),
            StartTime = _mainClockLastStartedAt,
            StopTime = DateTime.Now
        };
        int result = sqLiteConnection.Insert(workItem);
        switch (result)
        {
            case < 1:
                // something went wrong
                throw new InvalidOperationException("Expected 1 update, but 0 were effected");
            case > 1:
                // more updates than were expected
                throw new InvalidOperationException($"Expected 1 update, but {result} were effected");
        }
        
        
    }

    private void WorkItemEntry_OnCompleted(object sender, EventArgs e)
    {
        // user has pressed Enter on textbox, so start the clock
        Entry entry = (Entry)sender;

        // if the clock isnot running, start it
        if (_mainClock is { IsRunning: false })
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
        if (_mainClock == null)
        {
            if (Application.Current != null) _mainClock = Application.Current.Dispatcher.CreateTimer();
        }

        if (_mainClock == null) throw new NullReferenceException("Failed to define _mainClock");
        
        _mainClock.Interval = TimeSpan.FromSeconds(1);
        _mainClock.Tick += MainClock_Tick;

        StartStopButton.Text = "Stop";
        _mainClock.Start();
        _mainClockLastStartedAt = DateTime.Now;
        //this.Resources["Timer"] = "00:00:01"; // setting a dynamic resource
    }

    private void StopMainClock()
    {
        if (_mainClock == null) return;
        
        _mainClock.Stop();
        _mainClock.Tick -= MainClock_Tick;
        WorkItemEntry.Text = "";
        StartStopButton.Text = "Start";
    }

    private void MainClock_Tick(object sender, EventArgs e)
    {
        ClockLabel.Text = CalculateMainTimerDifference().ToString("h\\:mm\\:ss");
    }

    private TimeSpan CalculateMainTimerDifference()
    {
        return DateTime.Now - _mainClockLastStartedAt;
    }
}
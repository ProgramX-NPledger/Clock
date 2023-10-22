namespace Clock.Maui;

public partial class MainPage : ContentPage
{
    private bool _clockIsRunning = false;

    public MainPage()
    {
        InitializeComponent();
    }



    private void StartStopButton_OnClicked(object sender, EventArgs e)
    {
        if (_clockIsRunning)
        {
            WorkItemEntry.Text = "";
            StartStopButton.Text = "Start";
            _clockIsRunning = false;
        }
        else
        {
            StartStopButton.Text = "Stop";
            _clockIsRunning = true;
        }
    }
}
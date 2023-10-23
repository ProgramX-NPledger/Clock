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
            this.Resources["Timer"] = "00:00:01"; // setting a dynamic resource
            _clockIsRunning = true;
        }
    }
}
using Clock.Maui.Model;

namespace Clock.Maui.EventArgs;

public class UpdateAvailableEventArgs : System.EventArgs
{
    public AvailableUpdateStatus AvailableUpdateStatus { get; set; }
    
}
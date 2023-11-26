using Clock.Maui.Model;

namespace Clock.Maui.EventArgs;

public class UpdateReadyEventArgs : System.EventArgs
{
    public byte[] Update { get; set; }
    public string FileName { get; set; }
    
}
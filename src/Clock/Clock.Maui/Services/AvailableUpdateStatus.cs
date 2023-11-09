namespace Clock.Maui.Services;

public class AvailableUpdateStatus
{
    public Version CurrentVersion { get; private set; }
    public Version LatestAvailableVersion { get; private set; }
    public bool IsPreRelease { get; private set; }
    
}
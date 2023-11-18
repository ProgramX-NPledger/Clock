

namespace Clock.Maui.Services;

public class AvailableUpdateStatus
{
    public Version CurrentVersion { get; internal set; }
    public Version LatestAvailableVersion { get; internal set; }
    public bool IsPreRelease { get; internal set; }
    public DateTime? VersionCheckTime { get; set; }
    public bool CheckSuccessful { get; set; }

    public Exception Exception { get; set; }

    public string DownloadUrl { get; set; }
    

    public bool IsUpdateAvailable()
    {
        return CurrentVersion < LatestAvailableVersion;
    }
}
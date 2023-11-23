

using System.Reflection;
using Clock.Maui.Model.GitHub;

namespace Clock.Maui.Model;

public class AvailableUpdateStatus
{
    public AvailableRelease AvailableRelease { get; set; }
    public Version CurrentVersion { get; internal set; }
    public DateTime? VersionCheckTime { get; set; }
    public bool CheckSuccessful { get; set; }

    public Exception Exception { get; set; }



    public bool IsUpdateAvailable()
    {
        if (AvailableRelease == null) return false;
        return CurrentVersion < AvailableRelease.Version;
    }

}
namespace Clock.Maui.Services;

public class GitHubUpdateService
{
    public const string AUTOUPDATE_ENABLED_CONFIG_STRING = "autoupdate.enabled";
    public const string PREFER_PRERELEASE_CONFIG_STRING = "autoupdate.prefer-prerelease";
    
    public GitHubUpdateService()
    {
        
    }

    public AvailableUpdateStatus GetUpdateStatus(bool isPreRelease)
    {
        
        // get current update channel    
        
    }
    
    
}
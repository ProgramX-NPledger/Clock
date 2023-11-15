using System.Collections;
using System.Net.Http.Json;
using System.Text.Json;
using System.Collections.Generic;
using System.Reflection;
using Clock.Maui.Model.GitHub;

namespace Clock.Maui.Services;

public class GitHubUpdateService : IDisposable
{
    public const string AUTOUPDATE_ENABLED_CONFIG_STRING = "autoupdate.enabled";
    public const string PREFER_PRERELEASE_CONFIG_STRING = "autoupdate.prefer-prerelease";

    private readonly HttpClient _httpClient;

    
    public GitHubUpdateService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<AvailableUpdateStatus> GetUpdateStatus(bool allowPrerelease)
    {
        string url = "https://api.github.com/repos/ProgramX-NPledger/Clock/releases";

        AvailableUpdateStatus availableUpdateStatus = new AvailableUpdateStatus()
        {
            CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version,
            LatestAvailableVersion = Assembly.GetExecutingAssembly().GetName().Version,
            VersionCheckTime = DateTime.Now

        };

        IEnumerable<Release> releases;
        try
        {
            releases =
                await _httpClient.GetFromJsonAsync<IEnumerable<Release>>(url,
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }
        catch (Exception e)
        {
            availableUpdateStatus.Exception = e;
            availableUpdateStatus.CheckSuccessful = false;
            return availableUpdateStatus;
        }
        releases = releases.Where(q => (q.IsPreRelease && allowPrerelease || !q.IsPreRelease));
        
        // get latest version
        if (releases.Any())
        {
            IEnumerable<Release> latestRelease = releases.OrderByDescending(q => q.PublishedAt).Take(1);
            //availableUpdateStatus.LatestAvailableVersion = latestRelease.
        }
        else
        {
            availableUpdateStatus.CheckSuccessful = false;
        }

        return availableUpdateStatus;
    }


    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
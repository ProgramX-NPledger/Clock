using System.Collections;
using System.Net.Http.Json;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Clock.Maui.Factories;
using Clock.Maui.Model;
using Clock.Maui.Model.GitHub;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Newtonsoft.Json;
using OperatingSystem = Clock.Maui.Model.OperatingSystem;

namespace Clock.Maui.Services;

public class GitHubUpdateService : IDisposable
{
    public const string AUTOUPDATE_ENABLED_CONFIG_STRING = "autoupdate.enabled";
    public const string PREFER_PRERELEASE_CONFIG_STRING = "autoupdate.prefer-prerelease";

    private readonly HttpClient _httpClient;

    
    public GitHubUpdateService()
    {
        HttpClientHandler httpClientHandler = new HttpClientHandler();
        _httpClient = new HttpClient(httpClientHandler);
    }

    public async Task<AvailableUpdateStatus> GetUpdateStatus(bool allowPrerelease)
    {
        string url = "https://api.github.com/repos/ProgramX-NPledger/Clock/releases";

        AvailableUpdateStatus availableUpdateStatus = new AvailableUpdateStatus()
        {
            CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version,
            VersionCheckTime = DateTime.Now
        };

        IEnumerable<Release> releases=null;
        try
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            httpRequestMessage.Headers.Connection.Add("keep-alive");
            httpRequestMessage.Headers.UserAgent.Add(new ProductInfoHeaderValue("Clock","0.0"));
            
            var task = _httpClient.SendAsync(httpRequestMessage)
                .ContinueWith(async (taskwithmsg) =>
                {
                    HttpResponseMessage response = taskwithmsg.Result;
                    string releasesJson = await response.Content.ReadAsStringAsync();
                    releases = JsonConvert.DeserializeObject<Release[]>(releasesJson);
                });
            task.Wait();
        }
        catch (Exception e)
        {
            availableUpdateStatus.Exception = e;
            availableUpdateStatus.CheckSuccessful = false;
            return availableUpdateStatus;
        }
        
        // convert all releases to our known and parsed structure
        IEnumerable<AvailableRelease> allAvailableUpdates =
            releases.Select(AvailableRelease.FromRelease);
        
        // limit by prerelease preference
        IEnumerable<AvailableRelease> afterPreReleasePreference = 
            allAvailableUpdates.Where(q => (q.IsPreRelease && allowPrerelease || !q.IsPreRelease));

        // limit by architecture/os
        IEnumerable<AvailableRelease> afterPlatform = GetAvailableUpdateStatusByOperatingSystemAndArchitecture(afterPreReleasePreference);
        
        // get the latest
        if (afterPlatform.Any())
        {
            AvailableRelease latestAvailableUpdateStatus =
                afterPlatform.MaxBy(q => q.Version);

            availableUpdateStatus.AvailableRelease = latestAvailableUpdateStatus;
            availableUpdateStatus.CheckSuccessful = true;
        }
        else
        {
            availableUpdateStatus.CheckSuccessful = false;
        }

        return availableUpdateStatus;
    }

    private IEnumerable<AvailableRelease> GetAvailableUpdateStatusByOperatingSystemAndArchitecture(IEnumerable<AvailableRelease> updateStatuses)
    {
        if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
        {
            return updateStatuses.Where(q => q.OperatingSystem == OperatingSystem.Windows);
        } else if (DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
        {
            // TODO: return according to architecture
        }

        throw ExceptionsFactory.CreateDeviceNotSupportedException();
    }


    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
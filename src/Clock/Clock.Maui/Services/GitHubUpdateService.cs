using System.Collections;
using System.Net.Http.Json;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Clock.Maui.Model;
using Clock.Maui.Model.GitHub;
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
            LatestAvailableVersion = Assembly.GetExecutingAssembly().GetName().Version,
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
        releases = releases.Where(q => (q.IsPreRelease && allowPrerelease || !q.IsPreRelease));
        
        // get latest version
        if (releases.Any())
        {
            Release latestRelease = releases.OrderByDescending(q => q.PublishedAt).FirstOrDefault();
            availableUpdateStatus.CheckSuccessful = true;
            if (latestRelease == null)
            {
                availableUpdateStatus.LatestAvailableVersion = availableUpdateStatus.CurrentVersion;
                availableUpdateStatus.IsPreRelease = false;
            }
            else
            {
                PopulateAvailableUpdateStatus(ref availableUpdateStatus,latestRelease.TagName);
                availableUpdateStatus.IsPreRelease = latestRelease.IsPreRelease;
                Asset downloadableAsset =
                    latestRelease.Assets.FirstOrDefault(q => q.ContentType == "application/x-zip-compressed");
                if (downloadableAsset != null)
                {
                    availableUpdateStatus.DownloadUrl = downloadableAsset.BrowserDownloadUrl;
                }
                
            }
        }
        else
        {
            availableUpdateStatus.CheckSuccessful = false;
        }

        return availableUpdateStatus;
    }

    private void PopulateAvailableUpdateStatus(ref AvailableUpdateStatus availableUpdateStatus, string latestReleaseTagName)
    {
        // version string looks like: 
        string pattern = @"^v([0-9]\.[0.9]\.[0-9])\-(windows|macos)-(x86|m)-([a-z0-9]+)(\.([0-9]*?))$";
        RegexOptions options = RegexOptions.Singleline;

        MatchCollection matches = Regex.Matches(latestReleaseTagName, pattern, options);
        if (matches.Any())
        {
            Match firstMatch = matches.First();
            if (firstMatch.Groups.Count >= 4)
            {
                Group match2 = firstMatch.Groups[1]; // 	0.0.1
                Group match3 = firstMatch.Groups[2]; // windows|macos
                Group match4 = firstMatch.Groups[3]; // x86|m
                string versionAsString = $"{match2.Value}.0";
                if (Version.TryParse(versionAsString, out Version version))
                {
                    availableUpdateStatus.LatestAvailableVersion = version;
                    availableUpdateStatus.OperatingSystem = GetOperatingSystemFromString(match3.Value);
                    availableUpdateStatus.Architecture = GetArchitectureFromString(match4.Value);
                    return;
                }

                throw new InvalidOperationException($"Unable to extract version number from '{versionAsString}'");
            }
        }

        throw new InvalidOperationException($"No version number in string '{latestReleaseTagName}'");
    }

    private Architecture GetArchitectureFromString(string s)
    {
        if (s == null) throw new ArgumentNullException(nameof(s));
        switch (s)
        {
            case "x86":
                return Architecture.x86;
            case "m":
                return Architecture.M;
        }

        throw new NotSupportedException($"Unknown Architecture token '{s}'");
    }

    private OperatingSystem GetOperatingSystemFromString(string s)
    {
        if (s == null) throw new ArgumentNullException(nameof(s));
        switch (s)
        {
            case "windows":
                return OperatingSystem.Windows;
            case "macos":
                return OperatingSystem.macOS;
        }

        throw new NotSupportedException($"Unknown Operating System token '{s}'");
    }


    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
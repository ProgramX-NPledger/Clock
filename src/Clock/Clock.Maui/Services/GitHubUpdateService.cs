﻿using System.Collections;
using System.Net.Http.Json;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json.Nodes;
using Clock.Maui.Model.GitHub;
using Newtonsoft.Json;

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
            availableUpdateStatus.LatestAvailableVersion = null;
            availableUpdateStatus.IsPreRelease = latestRelease.IsPreRelease;
            Asset downloadableAsset =
                latestRelease.Assets.FirstOrDefault(q => q.ContentType == "application/x-zip-compressed");
            if (downloadableAsset != null)
            {
                availableUpdateStatus.DownloadUrl = downloadableAsset.BrowserDownloadUrl;
            }
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


using System.Reflection;
using System.Text.RegularExpressions;
using Clock.Maui.Model.GitHub;

namespace Clock.Maui.Model;

public class AvailableRelease
{
    internal const string ASSET_CONTENT_TYPE = "application/x-zip-compressed";
    
    public Version Version { get; internal set; }
    public bool IsPreRelease { get; internal set; }

    public string DownloadUrl { get; set; }

    public Architecture Architecture { get; set; }

    public OperatingSystem OperatingSystem { get; set; }



    public static AvailableRelease FromRelease(Release release)
    {
        if (release == null) throw new ArgumentNullException("release");

        AvailableRelease availableRelease = new AvailableRelease();

        string pattern = @"^v([0-9]\.[0.9]\.[0-9])\-(windows|macos)-(x86|m)-([a-z0-9]+)(\.([0-9]*?))$";
        RegexOptions options = RegexOptions.Singleline;

        MatchCollection matches = Regex.Matches(release.TagName, pattern, options);
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
                    availableRelease.IsPreRelease = release.IsPreRelease;
                    availableRelease.Version = version;
                    availableRelease.OperatingSystem = GetOperatingSystemFromString(match3.Value);
                    availableRelease.Architecture = GetArchitectureFromString(match4.Value);
                    availableRelease.DownloadUrl = GetDownloadUrlFromAssets(release?.Assets);
                    
                    return availableRelease;
                }

                throw new InvalidOperationException($"Unable to extract version number from '{versionAsString}'");
            }
        }

        throw new InvalidOperationException($"No version number in string '{release.TagName}'");
    }

    private static string GetDownloadUrlFromAssets(IEnumerable<Asset> releaseAssets)
    {
        if (releaseAssets == null) throw new ArgumentNullException(nameof(releaseAssets));
        
        Asset downloadableAsset =
            releaseAssets.FirstOrDefault(q => q.ContentType == ASSET_CONTENT_TYPE);
        if (downloadableAsset != null)
        {
            return downloadableAsset.BrowserDownloadUrl;
        }

        throw new InvalidOperationException($"Unable to resolve appropriate asset by content-type '{ASSET_CONTENT_TYPE};");
    }


    private static Architecture GetArchitectureFromString(string s)
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

    private static OperatingSystem GetOperatingSystemFromString(string s)
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

}
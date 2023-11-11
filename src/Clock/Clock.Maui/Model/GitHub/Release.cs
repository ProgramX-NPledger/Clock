﻿using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Clock.Maui.Model.GitHub;

public class Release
{
    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("assets_url")]
    public string AssetsUrl { get; set; }

    [JsonProperty("upload_url")] 
    public string UploadUrl { get; set; }

    [JsonProperty("html_url")]
    public string HtmlUrl { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }
    
    [JsonProperty("author")] 
    public Author Author { get; set; }
    
    [JsonProperty("node_id")]
    public string NodeId { get; set; }
    
    [JsonProperty("target_commitish")]
    public string TargetComitish { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("draft")]
    public bool IsDraft { get; set; }
    
    [JsonProperty("prerelease")]
    public bool IsPreRelease { get; set; }
    
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [JsonProperty("published_at")]
    public DateTime PublishedAt { get; set; }
    
    [JsonProperty("assets")]
    public IEnumerable<Asset> Assets { get; set; }
    
    [JsonProperty("tarball_url")]
    public string TarballUrl { get; set; }
    
    [JsonProperty("zipball_url")]
    public string ZipballUrl { get; set; }
    
    [JsonProperty("body")]
    public string Body { get; set; }
    
}
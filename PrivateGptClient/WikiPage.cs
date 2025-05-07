using Newtonsoft.Json;

public class WikiPage
{
    [JsonProperty("content")]
    public string Content { get; set; }

    [JsonProperty("gitItemPath")]
    public string GitItemPath { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("isNonConformant")]
    public bool IsNonConformant { get; set; }

    [JsonProperty("isParentPage")]
    public bool IsParentPage { get; set; }

    [JsonProperty("order")]
    public int Order { get; set; }

    [JsonProperty("path")]
    public string Path { get; set; }

    [JsonProperty("remoteUrl")]
    public string RemoteUrl { get; set; }

    [JsonProperty("subPages")]
    public List<WikiPage> SubPages { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }
}

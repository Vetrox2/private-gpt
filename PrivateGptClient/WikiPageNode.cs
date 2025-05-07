using Newtonsoft.Json;

public class WikiPageNode
{
    [JsonProperty("path")]
    public string Path { get; set; }

    [JsonProperty("order")]
    public int Order { get; set; }

    [JsonProperty("gitItemPath")]
    public string GitItemPath { get; set; }

    [JsonProperty("isParentPage")]
    public bool? IsParentPage { get; set; }

    [JsonProperty("subPages")]
    public List<WikiPageNode> SubPages { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("remoteUrl")]
    public string RemoteUrl { get; set; }
}

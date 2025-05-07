using Newtonsoft.Json;
using PrivateGptClient;
using System.Net.Http.Headers;
using System.Text;
const string ApiUrl = "http://127.0.0.1:8001/v1";
PrivateGptClient.PrivateGptClient clientGpt = new(ApiUrl);

Console.Write("1. Local files\n2. Azure wiki\nChoose:  ");
var choice = Console.ReadLine();
switch (choice)
{
    case "1":
        {
            Console.Write("Path: ");
            var FilesDir = Console.ReadLine();
            if (Path.Exists(FilesDir))
                await clientGpt.IngestFilesAsync(FilesDir);
        }
        break;
    case "2":
        {
            var org = "{{Org}}";
            var project = "[[ProjectName}}";
            var wikiId = "{{WikiId}";
            var pat = "{{Key}}";

            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($":{pat}"));
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", credentials);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            string url = $"https://dev.azure.com/{org}/{project}/_apis/wiki/wikis/{wikiId}/pages?recursionLevel=Full&includeContent=true&api-version=7.1-preview.1";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();

            var allPages = JsonConvert.DeserializeObject<WikiPage>(json);
            foreach (var page in allPages.SubPages)
            {
                var encodedPath = Uri.EscapeDataString(page.Path);
                var pageUrl = $"https://dev.azure.com/{org}/{project}/_apis/wiki/wikis/{wikiId}/pages?path={encodedPath}&includeContent=true&api-version=7.1-preview.1";
                var pageResp = await client.GetAsync(pageUrl);
                pageResp.EnsureSuccessStatusCode();
                var pageJson = await pageResp.Content.ReadAsStringAsync();
                var fullPage = JsonConvert.DeserializeObject<WikiPage>(pageJson);
                await clientGpt.IngestTextAsync(fullPage.Content, fullPage.Path);
            }

        }
        break;
    default: break;
}
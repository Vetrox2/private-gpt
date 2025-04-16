using PrivateGptClient;
const string ApiUrl = "http://127.0.0.1:8001/v1";
Console.Write("Path: ");
var FilesDir = Console.ReadLine();
PrivateGptClient.PrivateGptClient client = new(ApiUrl);
if (Path.Exists(FilesDir))
    await client.IngestFilesAsync(FilesDir);
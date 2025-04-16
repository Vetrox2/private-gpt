using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateGptClient
{
    public class PrivateGptClient(string apiUrl)
    {
        private readonly HttpClient _httpClient = new();
        private readonly string _apiUrl = apiUrl;

        public async Task IngestFilesAsync(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Directory not found: {directoryPath}");
                return;
            }

            try
            {
                // Get all files in the directory and all subdirectories recursively
                var files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);
                foreach (var filePath in files)
                {
                    await IngestFileAsync(filePath);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access denied to a directory or file: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IO error while accessing files: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        private async Task IngestFileAsync(string filePath)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                using var fileStream = File.OpenRead(filePath);
                using var fileContent = new StreamContent(fileStream);
                content.Add(fileContent, "file", Path.GetFileName(filePath));

                var response = await _httpClient.PostAsync($"{_apiUrl}/ingest", content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"File ingested: {filePath}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Failed to ingest file {filePath}: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error reading file {filePath}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error ingesting file {filePath}: {ex.Message}");
            }
        }
    }
}

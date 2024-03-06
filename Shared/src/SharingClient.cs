using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BackupUtilities.Shared;

public static partial class SharingClient
{
    public const string BASE = "icy.cx/bu";

    [GeneratedRegex("^(?:https://)?icy.cx/bu/([-_a-zA-Z0-9]{5})$")]
    public static partial Regex UriRegex();

    /// <summary>
    /// The base uri for the sharing service
    /// </summary>
    public static Uri BaseUri => new($"https://{BASE}");

    /// <summary>
    /// Get a config from the sharing service
    /// </summary>
    /// <param name="id">The id of the config</param>
    /// <returns>An array of BackupJob objects from the config</returns>
    public static async Task<BackupJob[]> Get(string id)
    {
        Uri uri = new($"https://{BASE}/{id}");

        using var client = new HttpClient();

        var response = await client.GetAsync(uri);
        response.EnsureSuccessStatusCode();

        return JsonSerializer.Deserialize<BackupJob[]>(
            await response.Content.ReadAsStringAsync(),
            Json.SerializerOptions
        )!;
    }

    /// <summary>
    /// Parse the id from a sharing service uri
    /// </summary>
    /// <param name="uri">The uri to parse</param>
    /// <returns>The id of the config or null if the uri is invalid</returns>
    public static string? ParseIdFromUri(string uri)
    {
        var match = UriRegex().Match(uri);
        if (!match.Success)
            return null;

        return match.Groups[1].Value;
    }

    private struct UploadResponse
    {
        public string Id { get; set; }
    }

    /// <summary>
    /// Upload a config to the sharing service
    /// </summary>
    /// <param name="jobs">The backup jobs to upload</param>
    /// <returns>The id of the uploaded config</returns>
    public static async Task<string> Upload(BackupJob[] jobs)
    {
        using var client = new HttpClient();

        var content = new StringContent(
            JsonSerializer.Serialize(jobs, Json.SerializerOptions),
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.PostAsync(BaseUri, content);
        response.EnsureSuccessStatusCode();

        var json = JsonSerializer.Deserialize<UploadResponse>(
            await response.Content.ReadAsStringAsync(),
            Json.SerializerOptions
        );

        return json.Id;
    }
}

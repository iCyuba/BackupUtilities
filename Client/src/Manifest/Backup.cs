using System.Text.Json;
using BackupUtilities.Shared;

namespace BackupUtilities.Client;

public class BackupManifest(DateTime date, BackupJob.BackupMethod method) : Manifest(method)
{
    /// <summary>
    /// The date of the backup
    /// </summary>
    public DateTime Date { get; set; } = date;

    /// <summary>
    /// The file hashes. Null if the file was deleted
    /// </summary>
    public Dictionary<string, byte[]?> Files { get; set; } = [];

    /// <summary>
    /// Load the backup manifest from a target and a backup id
    /// </summary>
    /// <param name="target">The target path</param>
    /// <param name="id">The backup id</param>
    public static BackupManifest Load(string target, Guid id)
    {
        string path = Path.Combine(target, $"{id}.json");
        if (!File.Exists(path))
            throw new FileNotFoundException("Backup manifest not found", path);

        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<BackupManifest>(json, Json.SerializerOptions)!;
    }

    /// <summary>
    /// Save the backup manifest to a target and a backup id
    /// </summary>
    /// <param name="target">The target path</param>
    /// <param name="id">The backup id</param>
    public void Save(string target, Guid id)
    {
        string path = Path.Combine(target, $"{id}.json");

        // Serialize the manifest and save it
        File.WriteAllText(path, JsonSerializer.Serialize(this, Json.SerializerOptionsPretty));
    }
}

using System.Text.Json;

namespace BackupUtilities.Client;

public class PackageManifest(BackupJob.BackupMethod method, Guid full) : Manifest(method)
{
    /// <summary>
    /// The full backup id
    ///
    /// Will be used by as the base for the partial backups.
    /// Or as the only backup if the method is full.
    /// </summary>
    public Guid Full { get; set; } = full;

    /// <summary>
    /// A list of all partial backup ids
    /// </summary>
    public List<Guid> Other { get; set; } = [];

    /// <summary>
    /// Load the package manifest from a target
    /// </summary>
    /// <param name="target">The target path</param>
    public static List<PackageManifest> Load(string target)
    {
        List<PackageManifest>? packages = null;

        string path = Path.Combine(target, "manifest.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            packages = JsonSerializer.Deserialize<List<PackageManifest>>(
                json,
                JsonUtils.SerializerOptions
            );
        }

        // If the manifest file doesn't exist, create a new one
        packages ??= [];

        return packages;
    }

    /// <summary>
    /// Save the package manifest to a target
    /// </summary>
    /// <param name="target">The target path</param>
    /// <param name="packages">The package manifest</param>
    public static void Save(string target, IEnumerable<PackageManifest> packages)
    {
        string path = Path.Combine(target, "manifest.json");

        // Serialize the manifest and save it
        File.WriteAllText(path, JsonSerializer.Serialize(packages, JsonUtils.SerializerOptions));
    }
}

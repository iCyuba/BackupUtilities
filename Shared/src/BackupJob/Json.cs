using System.Text.Json;
using NJsonSchema;

namespace BackupUtilities.Shared;

public partial class BackupJob
{
    private const string CONFIG_SCHEMA = "BackupUtilities.Shared.config.schema.json";

    /// <summary>
    /// The config schema.
    /// </summary>
    private static readonly JsonSchema Schema = LoadConfigSchema();

    /// <summary>
    /// Load the backup jobs from the config file.
    /// </summary>
    /// <param name="file">The path to the config file.</param>
    /// <param name="validate">Whether to validate the config file.</param>
    /// <returns>An array of BackupJob objects.</returns>
    /// <exception cref="JsonException" />
    public static BackupJob[] LoadFromConfig(string file, bool validate = true)
    {
        var json = File.ReadAllText(file);

        // Validate JSON using the config schema
        var errors = Schema.Validate(json);
        if (errors.Count > 0 && validate)
            throw new JsonException("Invalid config file: " + errors.First().ToString());

        // Parse JSON into Config object
        return JsonSerializer.Deserialize<BackupJob[]>(json, Json.SerializerOptions)!;
    }

    /// <summary>
    /// Load the config schema.
    /// </summary>
    private static JsonSchema LoadConfigSchema()
    {
        var stream = typeof(BackupJob).Assembly.GetManifestResourceStream(CONFIG_SCHEMA)!;

        return JsonSchema.FromJsonAsync(stream).Result;
    }
}

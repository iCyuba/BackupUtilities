using System.Text.Json;
using NJsonSchema;

namespace BackupUtility
{
    public partial class BackupJob
    {
        private const string CONFIG_SCHEMA = "BackupUtility.src.schemas.config.json";
        private const string CONFIG_FILE = "config/config.json";

        /// <summary>
        /// The config schema.
        /// </summary>
        private static readonly JsonSchema Schema = LoadConfigSchema();

        /// <summary>
        /// Load the backup jobs from the config file.
        /// </summary>
        /// <param name="file">The path to the config file.</param>
        /// <returns>An array of BackupJob objects.</returns>
        /// <exception cref="JsonException" />
        public static BackupJob[] LoadFromConfig(string file = CONFIG_FILE)
        {
            var json = File.ReadAllText(file);

            // Validate JSON using the config schema
            var errors = Schema.Validate(json);
            if (errors.Count > 0)
                throw new JsonException("Invalid config file: " + errors.First().ToString());

            // Parse JSON into Config object
            return JsonSerializer.Deserialize<BackupJob[]>(json, JsonUtils.SerializerOptions)!;
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
}

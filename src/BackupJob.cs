using System.Text.Json;
using System.Text.Json.Serialization;
using NJsonSchema;

namespace BackupUtility
{
    public class BackupJob
    {
        // Constants for the config file and schema

        private const string CONFIG_SCHEMA = "BackupUtility.src.config.schema.json";
        private const string CONFIG_FILE = "config/config.json";

        // Static properties

        /// <summary>
        /// The config schema.
        /// </summary>
        private static readonly JsonSchema Schema = LoadConfigSchema();

        /// <summary>
        /// Serialization options for the config file.
        /// </summary>
        /// <remarks>
        /// This is used to convert the JSON property names to camelCase.
        /// </remarks>
        public static readonly JsonSerializerOptions SerializerOptions =
            new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };

        // Subtypes
        public enum BackupMethod
        {
            Full,
            Differential,
            Incremental
        }

        public struct BackupRetention
        {
            public int Count { get; set; }
            public int Size { get; set; }
        }

        // Properties

        /// <summary>
        /// The sources to backup.
        /// </summary>
        public List<string> Sources { get; set; }

        /// <summary>
        /// The targets to backup to.
        /// </summary>
        public List<string> Targets { get; set; }

        /// <summary>
        /// The backup method to use.
        /// </summary>
        public BackupMethod Method { get; set; }

        /// <summary>
        /// The cron expression to use for scheduling.
        /// </summary>
        public string Timing { get; set; }

        /// <summary>
        /// The retention policy to use.
        /// </summary>
        public BackupRetention Retention { get; set; }

        public BackupJob(
            List<string> sources,
            List<string> targets,
            BackupMethod method,
            string timing,
            BackupRetention retention
        )
        {
            Sources = sources;
            Targets = targets;
            Method = method;
            Timing = timing;
            Retention = retention;
        }

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
            return JsonSerializer.Deserialize<BackupJob[]>(json, SerializerOptions)!;
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

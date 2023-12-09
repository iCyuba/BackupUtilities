using System.Text.Json;
using System.Text.Json.Serialization;

namespace BackupUtility
{
    public static class JsonUtils
    {
        /// <summary>
        /// Default serializer options
        /// </summary>
        public static readonly JsonSerializerOptions SerializerOptions =
            new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };

        // I'm too lazy to write this multiple times...
        /// <summary>
        /// Save a file as JSON
        /// </summary>
        /// <param name="path">The path to save the file to</param>
        /// <param name="obj">The object to serialize</param>
        /// <typeparam name="T">The type of the object</typeparam>
        public static void Save<T>(string path, T obj)
        {
            // Create the directory if it doesn't exist
            // (ik dir != null doesn't check if it exists, getDirName returns null if the path is invalid)
            string? dir = Path.GetDirectoryName(path);
            if (dir != null)
                Directory.CreateDirectory(dir);

            // Serialize the manifest and save it
            File.WriteAllText(Path.Combine(path), JsonSerializer.Serialize(obj, SerializerOptions));
        }

        /// <summary>
        /// Load a file as JSON
        /// </summary>
        /// <param name="path">The path to load the file from</param>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <returns>The deserialized object</returns>
        public static T Load<T>(string path)
        {
            // Check if the file exists
            if (!File.Exists(path))
                throw new FileNotFoundException("The file doesn't exist", path);

            // Deserialize the manifest
            return JsonSerializer.Deserialize<T>(File.ReadAllText(path), SerializerOptions)
                ?? throw new Exception("Failed to deserialize the manifest");
        }
    }
}

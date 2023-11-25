using System.Text.Json;

namespace BackupUtility
{
    public class Handler
    {
        // Properties
        public BackupJob Job { get; }
        public string Target { get; }
        public List<ManifestPackage> Manifest { get; }

        // Helpers
        private string ManifestPath => Path.Combine(Target, "manifest.json");

        public Handler(BackupJob job, string target)
        {
            Job = job;
            Target = target;

            // Load the manifest file
            if (File.Exists(ManifestPath))
                Manifest =
                    JsonSerializer.Deserialize<List<ManifestPackage>>(
                        File.ReadAllText(ManifestPath),
                        Json.SerializerOptions
                    ) ?? new();

            // If the manifest file doesn't exist, create a new one
            Manifest ??= new();
        }

        public void Backup(Guid id)
        {
            if (Job.Method != BackupJob.BackupMethod.Full)
                throw new Exception("Only full backups are supported for now");

            // Get the files for the backup
            Dictionary<string, string> files = new();

            foreach (string source in Job.Sources)
                Hashes.Get(source, files);

            // Create a new backup
            var backup = new ManifestBackup(id, DateTime.Now, Job.Method) { Files = files };
            var package = new ManifestPackage(Job.Method, backup);

            // Add the package to the manifest
            Manifest.Add(package);

            // Save the manifest file
            SaveManifest();

            // Create the package directory
            string packagePath = Path.Combine(Target, id.ToString());
            Directory.CreateDirectory(packagePath);

            // Copy the files to the target
            foreach (var file in files)
            {
                string source = file.Key;
                string target = Path.Combine(packagePath, file.Value + ".bin");

                // Copy the file
                File.Copy(source, target);
            }
        }

        private void SaveManifest()
        {
            // Create the directory if it doesn't exist
            if (!Directory.Exists(Target))
                Directory.CreateDirectory(Target);

            // Serialize the manifest and save it
            File.WriteAllText(
                ManifestPath,
                JsonSerializer.Serialize(Manifest, Json.SerializerOptions)
            );
        }
    }
}

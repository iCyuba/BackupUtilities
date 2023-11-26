using System.Text.Json;

namespace BackupUtility
{
    public class Handler
    {
        // Properties
        public BackupJob Job { get; }
        public string Target { get; }
        public List<PackageManifest> Packages { get; }

        public Handler(BackupJob job, string target)
        {
            Job = job;
            Target = target;

            // Load the manifest file
            string path = Path.Combine(Target, "manifest.json");
            if (File.Exists(path))
                Packages =
                    JsonSerializer.Deserialize<List<PackageManifest>>(
                        File.ReadAllText(path),
                        Json.SerializerOptions
                    ) ?? new();

            // If the manifest file doesn't exist, create a new one
            Packages ??= new();
        }

        public void Backup(Guid id)
        {
            if (Job.Method != BackupJob.BackupMethod.Full)
                throw new Exception("Only full backups are supported for now");

            // Get all the files for the backup (convert to hashset to remove duplicates)
            HashSet<string> files = Job.Sources
                .SelectMany(Files.GetAllFiles)
                .Select(file => file.FullName)
                .ToHashSet();

            // Run the backup
            FullBackup(id, files);
        }

        public void FullBackup(Guid id, HashSet<string> files)
        {
            // Create a new package + backup
            var backup = new BackupManifest(DateTime.Now, Job.Method);
            var package = new PackageManifest(Job.Method, id);

            // Add the package to the manifest
            Packages.Add(package);

            // Save the manifest files
            SaveManifest(Target, Packages);
            SaveManifest(Target, backup, id.ToString());

            // Get the path to the backup
            string backupPath = Path.Combine(Target, id.ToString());

            // Copy the files to the target
            foreach (var file in files)
            {
                // Cut off the file root. On posix systems this is a slash, on windows it's a drive letter
                // Note: since windows can have many different root directories, conflicts may occur! (e.g. C:\ and D:\)
                string root =
                    Path.GetPathRoot(file)
                    ?? throw new Exception("File root is null, what system are you using?????");

                string target = Path.Combine(backupPath, file[root.Length..]);
                if (file.Contains(".vscode"))
                {
                    Console.WriteLine("vscode file", file, target);
                }

                // Make sure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(target)!);

                // Copy the file (overwrite will never be needed on posix systems, can't say the same for windows...)
                File.Copy(file, target, true);
            }
        }

        private static void SaveManifest<T>(string path, T manifest, string filename = "manifest")
        {
            // Create the directory if it doesn't exist
            Directory.CreateDirectory(path);

            // Serialize the manifest and save it
            File.WriteAllText(
                Path.Combine(path, filename + ".json"),
                JsonSerializer.Serialize(manifest, Json.SerializerOptions)
            );
        }
    }
}

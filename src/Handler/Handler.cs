// This file is a mess

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

            // Normalize the target path
            Target = Files.NormalizePath(target);

            // Load the manifest file
            string path = Path.Combine(Target, "manifest.json");
            if (File.Exists(path))
                Packages = Json.Load<List<PackageManifest>>(path);

            // If the manifest file doesn't exist, create a new one
            Packages ??= new();
        }

        public void Backup(Guid id)
        {
            // Get all the files for the backup (convert to hashset to remove duplicates)
            HashSet<string> files = Job.Sources
                .SelectMany(Files.GetAllFiles)
                .Select(file => file.FullName)
                .ToHashSet();

            // Get the package for the backup
            PackageManifest? package = Packages.FirstOrDefault();

            // Determine the appropriate backup method to use
            bool full =
                package == null
                || package.Method != Job.Method
                || package.Other.Count >= Job.Retention.Size;

            switch (Job.Method)
            {
                case BackupJob.BackupMethod.Differential when !full:
                case BackupJob.BackupMethod.Incremental when !full:
                    PartialBackup(id, package!, files);
                    break;
                default:
                    FullBackup(id, files);
                    break;
            }
        }

        public void FullBackup(Guid id, HashSet<string> files)
        {
            // Create a new package + backup
            var backup = new BackupManifest(DateTime.Now, BackupJob.BackupMethod.Full);
            var package = new PackageManifest(Job.Method, id);

            // Add the package to the manifest
            Packages.Add(package);

            // Update the manifest file
            Json.Save(Path.Combine(Target, "manifest.json"), Packages);

            // Get the path to the backup
            string backupPath = Path.Combine(Target, id.ToString());

            // Copy the files to the target
            foreach (var file in files)
            {
                // Get the path relative to the root. This will ensure that file paths are unique (only on posix systems tho)
                string relative = Path.GetRelativePath(Path.GetPathRoot(file)!, file);
                string target = Path.Combine(backupPath, relative);

                // Make sure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(target)!);

                // Copy the file (overwrite will never be needed on posix systems, can't say the same for windows...)
                File.Copy(file, target, true);

                // Hash the file so it can be compared later
                backup.Files[relative] = Files.HashFile(target);
            }

            // Save the backup manifest
            Json.Save(Path.Combine(Target, $"{id}.json"), backup);
        }

        public void PartialBackup(Guid id, PackageManifest package, HashSet<string> files)
        {
            // Get a list of the previous backups to use for the comparison
            List<Guid> backups = new() { package.Full };

            // If the backup method is incremental, add all the other backups to the list
            if (Job.Method == BackupJob.BackupMethod.Incremental)
                backups.AddRange(package.Other);

            // Get the hashes of already backed up files
            var hashes = backups
                .Select(backup => Json.Load<BackupManifest>(Path.Combine(Target, $"{backup}.json")))
                .SelectMany(backup => backup.Files)
                .GroupBy(file => file.Key, file => file.Value)
                .ToDictionary(file => file.Key, file => file.Last());

            // Create a new backup and add it to the package
            var backup = new BackupManifest(DateTime.Now, Job.Method);
            package.Other.Add(id);

            // Update the manifest file
            Json.Save(Path.Combine(Target, "manifest.json"), Packages);

            // Get the path to the backup
            string backupPath = Path.Combine(Target, id.ToString());

            // Copy the files to the target
            foreach (var file in files)
            {
                // Get the path relative to the root. This will ensure that file paths are unique (only on posix systems tho)
                string relative = Path.GetRelativePath(Path.GetPathRoot(file)!, file);

                // Hash the file for the comparison
                byte[] hash = Files.HashFile(file);

                // Get the old hash and remove it from the manifest
                // (just used for the comparison. not saved!)
                byte[]? oldHash = hashes.GetValueOrDefault(relative, null);
                hashes.Remove(relative);

                // Check if the file has changed. If it hasn't, skip it
                if (oldHash != null && oldHash!.SequenceEqual(hash))
                    continue;

                // Add the new hash to the manifest
                backup.Files[relative] = hash;

                // Get the target path
                string target = Path.Combine(backupPath, relative);

                // Make sure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(target)!);

                // Copy the modified file
                File.Copy(file, target, true);
            }

            // Set the files that were removed to null (but don't do double nulls)
            foreach (var file in hashes.Keys)
                if (hashes[file] != null)
                    backup.Files[file] = null;

            // Save the backup manifest
            Json.Save(Path.Combine(Target, $"{id}.json"), backup);
        }
    }
}

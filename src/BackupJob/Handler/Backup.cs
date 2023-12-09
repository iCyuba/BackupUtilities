// This file is a mess

namespace BackupUtility
{
    public partial class BackupJob
    {
        /// <summary>
        /// Perform a backup.
        /// </summary>
        /// <param name="target">The target path</param>
        /// <param name="id">An optional id for the backup</param>
        public void Backup(string target, Guid id = default)
        {
            // Normalize the target path
            target = PathUtils.NormalizePath(target);

            // Generate a new id for the backup if none was provided
            id = id == default ? Guid.NewGuid() : id;

            // Get all the files for the backup (convert to hashset to remove duplicates)
            HashSet<string> files = Sources
                .SelectMany(PathUtils.GetAllFiles)
                .Select(file => file.FullName)
                .ToHashSet();

            // Load the manifest file
            List<PackageManifest>? packages = null;

            string packagesPath = Path.Combine(target, "manifest.json");
            if (File.Exists(packagesPath))
                packages = JsonUtils.Load<List<PackageManifest>>(packagesPath);

            // If the manifest file doesn't exist, create a new one
            packages ??= new();

            // Get the package for the backup
            var package = packages.LastOrDefault();

            // Determine the appropriate backup method to use
            bool full = package?.Method != Method || package.Other.Count >= Retention.Size - 1;

            BackupManifest backup;
            switch (Method)
            {
                case BackupMethod.Differential when !full:
                case BackupMethod.Incremental when !full:
                    backup = PartialBackup(target, id, package!, files);

                    // Add the new backup to the package
                    package!.Other.Add(id);
                    break;
                default:
                    backup = FullBackup(target, id, files);

                    // Add the new backup to the package
                    packages.Add(new(Method, id));
                    break;
            }

            // Save the manifest files
            JsonUtils.Save(Path.Combine(target, "manifest.json"), packages);
            JsonUtils.Save(Path.Combine(target, $"{id}.json"), backup);
        }

        /// <summary>
        /// Perform a backup on all targets.
        /// </summary>
        /// <param name="id">An optional id for the backups</param>
        public void Backup(Guid id = default)
        {
            // Generate a new id for the backup, this is because we want to use the same id for all targets
            id = id == default ? Guid.NewGuid() : id;

            // Backup all the targets
            foreach (var target in Targets)
                Backup(target, id);
        }
    }
}

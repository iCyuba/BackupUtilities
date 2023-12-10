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
            packages ??= [];

            // Get the package for the backup
            var package = packages.LastOrDefault();

            // Determine the appropriate backup method to use
            bool full = package?.Method != Method || package.Other.Count >= Retention.Size - 1;

            // Create the target directory if it doesn't exist
            Directory.CreateDirectory(target);

            // Get the output for the backup
            IBackupOutput output = OutputType switch
            {
                BackupOutputType.Folder => new FolderOutput(target, id),
                BackupOutputType.Tar
                    => new TarOutput(File.Create(Path.Combine(target, $"{id}.tar"))),
                _ => throw new InvalidOperationException("Invalid backup output type")
            };

            BackupManifest backup;
            switch (Method)
            {
                case BackupMethod.Differential when !full:
                case BackupMethod.Incremental when !full:
                    backup = PartialBackup(output, target, package!, files);

                    // Add the new backup to the package
                    package!.Other.Add(id);
                    break;
                default:
                    backup = FullBackup(output, files);

                    // Add the new backup to the package
                    packages.Add(new(Method, id));
                    break;
            }

            // Dispose of the output (this will close the file if it's a tar output)
            // (can't use "using" cuz it's in the switch)
            output.Dispose();

            // Clean up old backups
            if (Retention.Size > 0 && packages.Count > Retention.Size)
            {
                // Get the packages and backups to remove
                var packagesToRemove = packages.Take(packages.Count - Retention.Size);
                var backupsToRemove = packagesToRemove
                    .SelectMany(package => new[] { package.Full }.Concat(package.Other))
                    .ToHashSet();

                // Remove the packages
                packages.RemoveRange(0, packagesToRemove.Count());

                // Remove the backups
                foreach (var backupId in backupsToRemove)
                {
                    // Remove the directories themselves (if they exist, they don't always have to...)
                    string backupPath = Path.Combine(target, backupId.ToString());
                    if (Directory.Exists(backupPath))
                        Directory.Delete(backupPath, true);

                    // If the output is a tar file, remove it
                    string tarPath = Path.Combine(target, $"{backupId}.tar");
                    if (File.Exists(tarPath))
                        File.Delete(tarPath);

                    // Remove the manifest file (this one should always exist)
                    File.Delete(Path.Combine(target, $"{backupId}.json"));
                }
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

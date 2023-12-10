namespace BackupUtility
{
    public partial class BackupJob
    {
        private BackupManifest FullBackup(IBackupOutput output, HashSet<string> files)
        {
            // Create a new backup manifest
            var backup = new BackupManifest(DateTime.Now, BackupMethod.Full);

            // Copy the files to the target
            foreach (var file in files)
            {
                // Get the path relative to the root. This will ensure that file paths are unique (only on posix systems tho)
                string relative = Path.GetRelativePath(Path.GetPathRoot(file)!, file);

                // Get the file info
                FileInfo info = new(file);

                // Add the file to the backup
                output.Add(info, relative);

                // Hash the file so it can be compared later
                backup.Files[relative] = info.GetHash();
            }

            // Return the backup manifest
            return backup;
        }
    }
}

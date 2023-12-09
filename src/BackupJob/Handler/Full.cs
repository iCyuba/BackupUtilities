namespace BackupUtility
{
    public partial class BackupJob
    {
        private BackupManifest FullBackup(string target, Guid id, HashSet<string> files)
        {
            // Create a new backup manifest
            var backup = new BackupManifest(DateTime.Now, BackupMethod.Full);

            // Get the path to the backup
            string backupPath = Path.Combine(target, id.ToString());

            // Copy the files to the target
            foreach (var file in files)
            {
                // Get the path relative to the root. This will ensure that file paths are unique (only on posix systems tho)
                string relative = Path.GetRelativePath(Path.GetPathRoot(file)!, file);
                string fileTarget = Path.Combine(backupPath, relative);

                // Make sure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(fileTarget)!);

                // Get the file info
                FileInfo info = new(file);

                // Copy the file (overwrite will never be needed on posix systems, can't say the same for windows...)
                info.CopyTo(fileTarget, true);

                // Hash the file so it can be compared later
                backup.Files[relative] = info.GetHash();
            }

            // Return the backup manifest
            return backup;
        }
    }
}

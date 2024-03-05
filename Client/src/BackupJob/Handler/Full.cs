namespace BackupUtilities.Client;

public partial class BackupJob
{
    private BackupManifest FullBackup(IOutput output, IEnumerable<FileInfo> files)
    {
        // Create a new backup manifest
        var backup = new BackupManifest(DateTime.Now, BackupMethod.Full);

        // Copy the files to the target
        foreach (var file in files)
        {
            try
            {
                // Get the path relative to the root. This will ensure that file paths are unique (only on posix systems tho)
                string relative = PathUtils.GetRelativePath(file);

                // Add the file to the backup
                output.Add(file);

                // Hash the file so it can be compared later
                backup.Files[relative] = file.GetHash();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Failed to backup file {file.FullName}: {e.Message}");
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        // Return the backup manifest
        return backup;
    }
}

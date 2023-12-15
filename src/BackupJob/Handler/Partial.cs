namespace BackupUtility;

public partial class BackupJob
{
    private BackupManifest PartialBackup(
        IOutput output,
        string target,
        PackageManifest package,
        IEnumerable<FileInfo> files
    )
    {
        // Get a list of the previous backups to use for the comparison
        List<Guid> backups = [package.Full];

        // If the backup method is incremental, add all the other backups to the list
        if (Method == BackupMethod.Incremental)
            backups.AddRange(package.Other);

        // Get the hashes of already backed up files
        var hashes = backups
            .Select(id => JsonUtils.Load<BackupManifest>(Path.Combine(target, $"{id}.json")))
            .SelectMany(backup => backup.Files)
            .GroupBy(file => file.Key, file => file.Value)
            .ToDictionary(file => file.Key, file => file.Last());

        // Create a new backup and add it to the package
        var backup = new BackupManifest(DateTime.Now, Method);

        // Copy the files to the target
        foreach (var file in files)
        {
            try
            {
                // Get the path relative to the root. This will ensure that file paths are unique (only on posix systems tho)
                string relative = PathUtils.GetRelativePath(file);

                // Get the old hash and remove it from the old hashes
                byte[]? oldHash = hashes.GetValueOrDefault(relative, null);
                hashes.Remove(relative);

                // Get the hash
                var hash = file.GetHash();

                // Check if the file has changed. If it hasn't, skip it
                if (oldHash != null && oldHash!.SequenceEqual(hash))
                    continue;

                // Add the new hash to the manifest
                backup.Files[relative] = hash;

                // Add the file to the backup
                output.Add(file);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Failed to backup file {file.FullName}: {e.Message}");
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        // Set the files that were removed to null (but don't do double nulls)
        foreach (var file in hashes.AsEnumerable())
            if (file.Value != null)
                backup.Files[file.Key] = null;

        // Return the backup manifest
        return backup;
    }
}

namespace BackupUtility;

public partial class BackupJob
{
    /// <summary>
    /// Cleanup old backups from the target based on the retention policy.
    ///
    /// Should be called after a backup has been made.
    /// </summary>
    /// <param name="packages">The packages to clean up</param>
    /// <param name="target">The target path</param>
    private void Cleaup(List<PackageManifest> packages, string target)
    {
        // Don't do anything if the retention policy is disabled or the packages aren't over the limit
        if (Retention.Count <= 0 || packages.Count <= Retention.Count)
            return;

        // Get the packages and backups to remove
        var packagesToRemove = packages.Take(packages.Count - Retention.Count);
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

            // Remove files that match the id
            foreach (var file in Directory.EnumerateFiles(target))
                if (Path.GetFileName(file).StartsWith(backupId.ToString()))
                    File.Delete(file);
        }
    }
}

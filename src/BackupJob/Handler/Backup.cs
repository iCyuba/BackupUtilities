using System.Text.RegularExpressions;

namespace BackupUtility;

public partial class BackupJob
{
    /// <summary>
    /// Perform a backup.
    /// </summary>
    /// <param name="target">The target path</param>
    /// <param name="id">An optional id for the backup</param>
    public void Backup(string target, Guid id = default)
    {
        // Normalize the target path and make sure it exists
        target = PathUtils.NormalizePath(target);
        Directory.CreateDirectory(target);

        // Generate a new id for the backup if none was provided
        id = id == default ? Guid.NewGuid() : id;

        // Get all the files for the backup
        var files = PathUtils
            .GetAllFiles(Sources)
            // Filter out files that match the ignore patterns
            .Where(file => !Ignore.Any(pattern => Regex.IsMatch(file.FullName, pattern)));

        // Load the manifest file and get the last package
        List<PackageManifest> packages = PackageManifest.Load(target);
        var package = packages.LastOrDefault();

        // Determine the appropriate backup method to use
        // If the package is null, we need to do a full backup
        // Or if the package is full, we need to do a full backup (if the limit is invalid, it will be ignored)
        bool full =
            package?.Method != Method
            || (Retention.Size > 0 && package.Other.Count >= Retention.Size - 1);

        // Get the output for the backup
        using var output = IOutput.GetOutput(Output, target, id);

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

        // Clean up old backups
        Cleaup(packages, target);

        // Save the manifest files
        PackageManifest.Save(target, packages);
        backup.Save(target, id);
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

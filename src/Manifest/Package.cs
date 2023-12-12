namespace BackupUtility;

public class PackageManifest(BackupJob.BackupMethod method, Guid full) : Manifest(method)
{
    /// <summary>
    /// The full backup id
    ///
    /// Will be used by as the base for the partial backups.
    /// Or as the only backup if the method is full.
    /// </summary>
    public Guid Full { get; set; } = full;

    /// <summary>
    /// A list of all partial backup ids
    /// </summary>
    public List<Guid> Other { get; set; } = [];
}

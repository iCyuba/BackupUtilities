using BackupUtilities.Shared;

namespace BackupUtilities.Client;

public abstract class Manifest(BackupJob.BackupMethod method)
{
    /// <summary>
    /// The method used for the backup
    /// </summary>
    public BackupJob.BackupMethod Method { get; set; } = method;
}

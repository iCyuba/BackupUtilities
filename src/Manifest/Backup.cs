namespace BackupUtility;

public class BackupManifest(DateTime date, BackupJob.BackupMethod method) : Manifest(method)
{
    /// <summary>
    /// The date of the backup
    /// </summary>
    public DateTime Date { get; set; } = date;

    /// <summary>
    /// The file hashes. Null if the file was deleted
    /// </summary>
    public Dictionary<string, byte[]?> Files { get; set; } = [];
}

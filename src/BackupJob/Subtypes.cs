namespace BackupUtility;

public partial class BackupJob
{
    /// <summary>
    /// Possible backup methods
    /// </summary>
    public enum BackupMethod
    {
        Full,
        Differential,
        Incremental
    }

    /// <summary>
    /// Retention policy
    /// </summary>
    public struct BackupRetention
    {
        public int Count { get; set; }
        public int Size { get; set; }
    }

    /// <summary>
    /// Output type
    /// </summary>
    public enum BackupOutput
    {
        Folder,
        Tar,
        TarGz,
        TarBz2,
        Zip
    }
}

namespace BackupUtility
{
    public class BackupManifest : Manifest
    {
        /// <summary>
        /// The date of the backup
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The file hashes. Null if the file was deleted
        /// </summary>
        public Dictionary<string, byte[]?> Files { get; set; } = new();

        public BackupManifest(DateTime date, BackupJob.BackupMethod method)
            : base(method)
        {
            Date = date;
            Method = method;
        }
    }
}

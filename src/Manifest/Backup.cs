namespace BackupUtility
{
    public class BackupManifest : Manifest
    {
        /// <summary>
        /// The date of the backup
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The files that were removed since the last backup
        /// </summary>
        public List<string> Removed { get; set; }

        public BackupManifest(DateTime date, BackupJob.BackupMethod method)
            : base(method)
        {
            Date = date;
            Method = method;
            Removed = new();
        }
    }
}

namespace BackupUtility
{
    public class ManifestBackup
    {
        /// <summary>
        /// The id of the backup
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The date of the backup
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The used method for the backup, doesn't have to be the same as the one in the root manifest!
        /// </summary>
        public BackupJob.BackupMethod Method { get; set; }

        /// <summary>
        /// The files that were backed up and their hashes
        /// </summary>
        public Dictionary<string, string> Files { get; set; }

        public ManifestBackup(Guid id, DateTime date, BackupJob.BackupMethod method)
        {
            Id = id;
            Date = date;
            Method = method;
            Files = new();
        }
    }
}

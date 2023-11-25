namespace BackupUtility
{
    public class ManifestPackage
    {
        /// <summary>
        /// The method used for the backup
        /// </summary>
        public BackupJob.BackupMethod Method { get; set; }

        /// <summary>
        /// The full backup of the package.
        ///
        /// Will be used by as the base for the incremental / differential backups.
        /// Or as the only backup if the method is full.
        /// </summary>
        public ManifestBackup Full { get; set; }

        /// <summary>
        /// A list of all the incremental / differential backups
        /// </summary>
        public List<ManifestBackup> Other { get; set; }

        public ManifestPackage(BackupJob.BackupMethod method, ManifestBackup full)
        {
            Method = method;
            Full = full;
            Other = new();
        }
    }
}

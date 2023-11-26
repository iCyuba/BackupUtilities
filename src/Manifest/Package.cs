namespace BackupUtility
{
    public class PackageManifest : Manifest
    {
        /// <summary>
        /// The full backup id
        ///
        /// Will be used by as the base for the incremental / differential backups.
        /// Or as the only backup if the method is full.
        /// </summary>
        public Guid Full { get; set; }

        /// <summary>
        /// A list of all incremental / differential backup ids
        /// </summary>
        public List<Guid> Other { get; set; }

        public PackageManifest(BackupJob.BackupMethod method, Guid full)
            : base(method)
        {
            Full = full;
            Other = new();
        }
    }
}

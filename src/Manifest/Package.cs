namespace BackupUtility
{
    public class PackageManifest : Manifest
    {
        /// <summary>
        /// The full backup id
        ///
        /// Will be used by as the base for the partial backups.
        /// Or as the only backup if the method is full.
        /// </summary>
        public Guid Full { get; set; }

        /// <summary>
        /// A list of all partial backup ids
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

namespace BackupUtility
{
    public abstract class Manifest
    {
        /// <summary>
        /// The method used for the backup
        /// </summary>
        public BackupJob.BackupMethod Method { get; set; }

        public Manifest(BackupJob.BackupMethod method)
        {
            Method = method;
        }
    }
}

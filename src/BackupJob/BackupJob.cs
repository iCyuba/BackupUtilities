namespace BackupUtility
{
    public partial class BackupJob(
        List<string> sources,
        List<string> targets,
        BackupJob.BackupMethod method,
        string timing,
        BackupJob.BackupRetention retention,
        BackupJob.BackupOutput output = BackupJob.BackupOutput.Folder
    )
    {
        /// <summary>
        /// The sources to backup.
        /// </summary>
        public List<string> Sources { get; set; } = sources;

        /// <summary>
        /// The targets to backup to.
        /// </summary>
        public List<string> Targets { get; set; } = targets;

        /// <summary>
        /// The backup method to use.
        /// </summary>
        public BackupMethod Method { get; set; } = method;

        /// <summary>
        /// The cron expression to use for scheduling.
        /// </summary>
        public string Timing { get; set; } = timing;

        /// <summary>
        /// The retention policy to use.
        /// </summary>
        public BackupRetention Retention { get; set; } = retention;

        /// <summary>
        /// Output type. Defaults to folder.
        /// </summary>
        public BackupOutput Output { get; set; } = output;
    }
}

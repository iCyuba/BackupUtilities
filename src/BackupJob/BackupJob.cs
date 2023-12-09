namespace BackupUtility
{
    public partial class BackupJob
    {
        /// <summary>
        /// The sources to backup.
        /// </summary>
        public List<string> Sources { get; set; }

        /// <summary>
        /// The targets to backup to.
        /// </summary>
        public List<string> Targets { get; set; }

        /// <summary>
        /// The backup method to use.
        /// </summary>
        public BackupMethod Method { get; set; }

        /// <summary>
        /// The cron expression to use for scheduling.
        /// </summary>
        public string Timing { get; set; }

        /// <summary>
        /// The retention policy to use.
        /// </summary>
        public BackupRetention Retention { get; set; }

        public BackupJob(
            List<string> sources,
            List<string> targets,
            BackupMethod method,
            string timing,
            BackupRetention retention
        )
        {
            Sources = sources;
            Targets = targets;
            Method = method;
            Timing = timing;
            Retention = retention;
        }
    }
}

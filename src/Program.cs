namespace BackupUtility
{
    internal class Program
    {
        static void Main()
        {
            // Load the backup jobs from the default config file
            var jobs = BackupJob.LoadFromConfig();

            foreach (var job in jobs)
            {
                var id = Guid.NewGuid();

                foreach (string target in job.Targets)
                {
                    // Create the backup
                    var backup = new Handler(job, target);

                    // Save the backup
                    backup.Backup(id);
                }
            }
        }
    }
}

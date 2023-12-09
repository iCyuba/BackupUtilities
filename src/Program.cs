namespace BackupUtility
{
    internal class Program
    {
        static async Task Main()
        {
            // Load the backup jobs from the default config file
            var jobs = BackupJob.LoadFromConfig();

            // Setup the backup scheduler
            await BackupScheduler.Setup(jobs);

            // Wait forever
            await Task.Delay(-1);
        }
    }
}

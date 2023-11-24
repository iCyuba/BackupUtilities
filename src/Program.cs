namespace BackupUtility
{
    internal class Program
    {
        static void Main()
        {
            // Load the backup jobs from the default config file
            var jobs = BackupJob.LoadFromConfig();
        }
    }
}

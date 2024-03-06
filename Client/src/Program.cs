using BackupUtilities.Shared;

namespace BackupUtilities.Client;

internal class Program
{
    /// <summary>
    /// A utility to backup files from one location to another.
    ///
    /// The utility is configured using a json file which contains a list of backup jobs.
    ///
    /// Each backup job should contain the following:
    /// - List of source directories
    /// - List of destination directories
    /// - List of files to exclude (optional)
    /// - Backup method (full, differential, incremental)
    /// - Retention policy (number of backups to keep)
    /// - Cron expression to use for scheduling
    /// - Output type (folder, tar, tar.gz, tar.bz2, zip) (optional, defaults to folder)
    /// </summary>
    /// <param name="config">Path to the config file</param>
    /// <param name="once">Run the backup once and exit</param>
    static async Task<int> Main(string config = "./config.json", bool once = false)
    {
        // Load the backup jobs from the default config file
        BackupJob[] jobs;
        try
        {
            jobs = BackupJob.LoadFromConfig(config);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return 1;
        }

        // If the once flag is set, run the backup once and exit
        if (once)
            try
            {
                jobs.ToList().ForEach(job => new BackupHandler(job).Backup());
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }

        // Otherwise, setup the scheduler and wait forever
        await BackupScheduler.Setup(jobs);
        await Task.Delay(-1);

        // Should never be reached...
        return 1;
    }
}

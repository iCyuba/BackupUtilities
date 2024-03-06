using BackupUtilities.Shared;
using Quartz;
using Quartz.Impl;

namespace BackupUtilities.Client;

/// <summary>
/// The backup scheduler.
///
/// This shouldn't be instantiated directly, use <see cref="Setup"/> instead.
/// </summary>
public static class BackupScheduler
{
    private class BackupSchedulerJob : IJob
    {
        /// <summary>
        /// Execute the backup job for the given trigger
        /// </summary>
        public Task Execute(IJobExecutionContext context)
        {
            // Get the job from the data map
            var job = (BackupHandler)context.MergedJobDataMap["job"];

            // Run the backup...
            job.Backup();

            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Setup the backup scheduler with the given jobs
    /// </summary>
    /// <param name="jobs">The jobs to schedule</param>
    public static async Task Setup(IEnumerable<BackupJob> jobs)
    {
        // Create the scheduler
        StdSchedulerFactory factory = new();
        IScheduler scheduler = await factory.GetScheduler();
        await scheduler.Start();

        // For each backup job, create a scheduler job and schedule it using the cron trigger
        foreach (var job in jobs)
        {
            // Create a scheduler job
            IJobDetail schedulerJob = JobBuilder.Create<BackupSchedulerJob>().Build();

            // Setup the cron trigger
            var trigger = TriggerBuilder
                .Create()
                .WithCronSchedule(FixCron(job.Timing))
                .StartNow()
                .Build();

            // Pass the backup handler in the job data map
            schedulerJob.JobDataMap["handler"] = new BackupHandler(job);

            // Schedule the job
            await scheduler.ScheduleJob(schedulerJob, trigger);
        }
    }

    /// <summary>
    /// Fix a cron expression to be valid.
    ///
    /// I blame Quartz for this...
    /// </summary>
    /// <param name="cron">The cron expression to fix</param>
    /// <returns>The fixed cron expression</returns>
    private static string FixCron(string cron)
    {
        // Split the cron expression into parts
        List<string> parts = [..cron.Split(' ')];

        // If there are less than 6 parts, add a 0 to the start (seconds)
        if (parts.Count < 6)
            parts.Insert(0, "0");

        // If day of month / week is * and the other is not, set it to ?
        if (parts[3] == "*" && parts[5] == "*" || parts[3] != "*" && parts[5] == "*")
            parts[5] = "?";
        else if (parts[3] == "*" && parts[5] != "*")
            parts[3] = "?";

        // Return the fixed cron expression
        return string.Join(' ', parts);
    }
}

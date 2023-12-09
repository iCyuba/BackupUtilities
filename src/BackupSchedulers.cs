using Quartz;
using Quartz.Impl;

namespace BackupUtility
{
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
                var job = (BackupJob)context.MergedJobDataMap["job"];

                // Make up a backup id
                var id = Guid.NewGuid();

                // Backup to each target
                foreach (string target in job.Targets)
                {
                    // Create a new backup handler
                    var handler = new Handler(job, target);

                    // Backup to the target
                    handler.Backup(id);
                }

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
                    .WithCronSchedule($"0 {job.Timing} *")
                    .StartNow()
                    .Build();

                // Pass the backup job in the job data map
                schedulerJob.JobDataMap["job"] = job;

                // Schedule the job
                await scheduler.ScheduleJob(schedulerJob, trigger);
            }
        }
    }
}

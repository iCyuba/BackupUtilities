using System.Text.Json.Serialization;

namespace BackupUtilities.Shared;

public partial class BackupJob(
    IEnumerable<string> sources,
    IEnumerable<string> targets,
    BackupJob.BackupMethod method,
    string timing,
    BackupJob.BackupRetention retention,
    IEnumerable<string>? ignore = default,
    BackupJob.BackupOutput output = BackupJob.BackupOutput.Folder
)
{
    /// <summary>
    /// The sources to backup.
    /// </summary>
    [Icon("")]
    public IEnumerable<string> Sources { get; set; } = sources;

    /// <summary>
    /// Ignored patterns.
    /// </summary>
    [Icon("")]
    public IEnumerable<string> Ignore { get; set; } = ignore ?? [];

    /// <summary>
    /// The targets to backup to.
    /// </summary>
    [Icon("")]
    public IEnumerable<string> Targets { get; set; } = targets;

    /// <summary>
    /// The backup method to use.
    /// </summary>
    [Icon("")]
    public BackupMethod Method { get; set; } = method;

    /// <summary>
    /// The cron expression to use for scheduling.
    /// </summary>
    [Icon("")]
    public string Timing { get; set; } = timing;

    /// <summary>
    /// The retention policy to use.
    /// </summary>
    [Icon("")]
    public BackupRetention Retention { get; set; } = retention;

    /// <summary>
    /// Output type. Defaults to folder.
    /// </summary>
    [Icon("")]
    [JsonConverter(typeof(OutputJsonConverter))]
    public BackupOutput Output { get; set; } = output;

    public BackupJob()
        : this([], [], BackupMethod.Full, "0 0 * * *", new()) { }
}

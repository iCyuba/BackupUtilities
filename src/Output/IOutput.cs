using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;

namespace BackupUtility;

public interface IOutput : IDisposable
{
    /// <summary>
    /// Add a file to the backup
    /// </summary>
    /// <param name="file">The file to add</param>
    public void Add(FileInfo file);

    /// <summary>
    /// Get the appropriate backup output for the given target
    /// </summary>
    /// <param name="type">The type of backup output to use</param>
    /// <param name="target">The target path</param>
    public static IOutput GetOutput(BackupJob.BackupOutput type, string target, Guid id)
    {
        if (type == BackupJob.BackupOutput.Folder)
            return new FolderOutput(target, id);

        // Get the appropriate file extension
        string extension = type.GetDescription().ToLowerInvariant();

        // Create the file
        Stream file = File.Create(Path.Combine(target, $"{id}.{extension}"));

        if (type == BackupJob.BackupOutput.Zip)
            return new ZipOutput(file);

        return new TarOutput(
            type switch
            {
                BackupJob.BackupOutput.Tar => file,
                BackupJob.BackupOutput.TarGz => new GZipOutputStream(file),
                BackupJob.BackupOutput.TarBz2 => new BZip2OutputStream(file),

                // Should never happen, but yk...
                _ => throw new ArgumentException("Invalid backup output type"),
            }
        );
    }
}

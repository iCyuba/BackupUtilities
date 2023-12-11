using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;

namespace BackupUtility
{
    public interface IBackupOutput : IDisposable
    {
        /// <summary>
        /// Add a file to the backup
        /// </summary>
        /// <param name="file">The file to add</param>
        /// <param name="path">The path to the file. Should be the relative path from the root of the backup</param>
        public void Add(FileInfo file, string path);

        /// <summary>
        /// Get the appropriate backup output for the given target
        /// </summary>
        /// <param name="type">The type of backup output to use</param>
        /// <param name="target">The target path</param>
        public static IBackupOutput GetOutput(BackupJob.BackupOutput type, string target, Guid id)
        {
            if (type == BackupJob.BackupOutput.Folder)
                return new FolderOutput(target, id);

            // Get the appropriate file extension
            string extension = type switch
            {
                BackupJob.BackupOutput.Tar => "tar",
                BackupJob.BackupOutput.TarGz => "tar.gz",
                BackupJob.BackupOutput.TarBz2 => "tar.bz2",
                BackupJob.BackupOutput.Zip => "zip",

                _ => throw new InvalidOperationException("Invalid backup output type"),
            };

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

                    // Will never happen cuz this gets caught above. But the compiler doesn't know that ig
                    _ => throw new Exception()
                }
            );
        }
    }
}

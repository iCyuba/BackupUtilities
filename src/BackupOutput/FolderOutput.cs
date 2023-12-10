namespace BackupUtility
{
    /// <summary>
    /// A backup output that stores the files in a folder on the local filesystem
    /// </summary>
    /// <param name="target">Path to the backup folder</param>
    /// <param name="id">The id of the backup</param>
    class FolderOutput(string target, Guid id) : IBackupOutput
    {
        private string OutputPath { get; } = Path.Combine(target, id.ToString());

        public void Add(FileInfo file, string path)
        {
            // Copy the file to the output path
            string newPath = Path.Combine(OutputPath, path);

            // Make sure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(newPath)!);

            // Copy the file (overwrite might be needed on windows, but not on posix systems)
            file.CopyTo(newPath, true);
        }

        // Nothing to do here. I just need compatibility with the tar output
        public void Dispose() { }
    }
}

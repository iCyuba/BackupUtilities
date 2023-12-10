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
    }
}

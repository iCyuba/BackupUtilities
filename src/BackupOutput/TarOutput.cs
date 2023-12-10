using ICSharpCode.SharpZipLib.Tar;

namespace BackupUtility
{
    /// <summary>
    /// A backup output, which writes to a tar archive
    /// </summary>
    /// <param name="outputStream">The stream to write the tar archive to</param>
    class TarOutput(Stream outputStream) : IBackupOutput, IDisposable
    {
        private TarOutputStream OutputStream { get; } = new TarOutputStream(outputStream, null);

        public void Add(FileInfo file, string path)
        {
            // Create a new tar entry
            TarEntry entry = TarEntry.CreateTarEntry(path);

            // Set the size of the entry
            entry.Size = file.Length;

            // Write the entry to the tar archive
            OutputStream.PutNextEntry(entry);

            // Open the file
            using var stream = file.OpenRead();

            // Copy the file to the tar archive
            stream.CopyTo(OutputStream);

            // Close the entry
            OutputStream.CloseEntry();
        }

        public void Dispose()
        {
            // Close the tar archive
            OutputStream.Close();
        }
    }
}

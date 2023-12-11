using ICSharpCode.SharpZipLib.Zip;

namespace BackupUtility
{
    /// <summary>
    /// A backup output, which writes to a zip file
    /// </summary>
    /// <param name="outputStream">The stream to write the zip file to</param>
    class ZipOutput(Stream outputStream) : IBackupOutput, IDisposable
    {
        private readonly ZipOutputStream OutputStream = new(outputStream);

        public void Add(FileInfo file, string path)
        {
            // Create a new zip entry
            ZipEntry entry =
                new(ZipEntry.CleanName(path)) { DateTime = file.LastWriteTime, Size = file.Length };

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

using ICSharpCode.SharpZipLib.Zip;

namespace BackupUtility;

/// <summary>
/// A backup output, which writes to a zip file
/// </summary>
/// <param name="outputStream">The stream to write the zip file to</param>
class ZipOutput(Stream outputStream) : IOutput, IDisposable
{
    private readonly ZipOutputStream _outputStream = new(outputStream);

    public void Add(FileInfo file)
    {
        // Skip the file, if it is a symbolic link (don't know how to handle them... sorry)
        if (file.LinkTarget != null)
            return;

        // Create a new zip entry
        ZipEntry entry =
            new(ZipEntry.CleanName(PathUtils.GetRelativePath(file)))
            {
                DateTime = file.LastWriteTime,
                Size = file.Length
            };

        // Write the entry to the tar archive
        _outputStream.PutNextEntry(entry);

        // Open the file
        using var stream = file.OpenRead();

        // Copy the file to the tar archive
        stream.CopyTo(_outputStream);

        // Close the entry
        _outputStream.CloseEntry();
    }

    public void Dispose()
    {
        // Close the tar archive
        _outputStream.Close();
    }
}

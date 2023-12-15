using ICSharpCode.SharpZipLib.Tar;

namespace BackupUtility;

/// <summary>
/// A backup output, which writes to a tar archive
/// </summary>
/// <param name="outputStream">The stream to write the tar archive to</param>
class TarOutput(Stream outputStream) : IOutput, IDisposable
{
    private readonly TarOutputStream _outputStream = new(outputStream, null);

    public void Add(FileInfo file)
    {
        // Skip the file, if it is a symbolic link (don't know how to handle them... sorry)
        if (file.LinkTarget != null)
            return;

        // Create a new tar entry from the path relative to the root
        string path = PathUtils.GetRelativePath(file).Replace('\\', '/');
        TarEntry entry = TarEntry.CreateTarEntry(path);

        // Set the size of the entry
        entry.Size = file.Length;

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

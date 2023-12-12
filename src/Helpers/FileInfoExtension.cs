using System.Security.Cryptography;

namespace BackupUtility;

public static class FileInfoExtension
{
    /// <summary>
    /// Hash the file contents using SHA256
    /// </summary>
    /// <param name="file">The file to hash</param>
    /// <returns>The hash of the file as a base64 string</returns>
    public static byte[] GetHash(this FileInfo file)
    {
        // Create a new hash algorithm
        using var hash = SHA256.Create();

        // Open the file
        using var stream = file.OpenRead();

        // Compute the hash
        return hash.ComputeHash(stream);
    }
}

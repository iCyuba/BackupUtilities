using System.Security.Cryptography;

namespace BackupUtility
{
    public static class Hashes
    {
        /// <summary>
        /// Get all files with their hashes in a directory and its subdirectories and add them to a dictionary
        /// </summary>
        /// <param name="directory">The file / directory to hash</param>
        /// <param name="files">An existing dictionary to add the files to</param>
        public static void Get(string path, Dictionary<string, string> files)
        {
            // Check if the path is a file
            if (File.Exists(path))
            {
                // Add the file to the dictionary
                files.Add(new FileInfo(path).FullName, HashFile(new FileInfo(path)));
                return;
            }

            // Otherwise assume it's a directory
            HashDirectory(new DirectoryInfo(path), files);
        }

        private static void HashDirectory(DirectoryInfo directory, Dictionary<string, string> files)
        {
            // Check if the source directory exists
            if (!directory.Exists)
                throw new DirectoryNotFoundException(
                    $"The directory {directory.FullName} does not exist or cannot be accessed"
                );

            // Add all files in the directory
            foreach (var file in directory.EnumerateFiles())
                files.Add(file.FullName, HashFile(file));

            // And add all subdirectories as well
            foreach (var subdirectory in directory.EnumerateDirectories())
                HashDirectory(subdirectory, files);
        }

        private static string HashFile(FileInfo file)
        {
            // Check if the source file exists
            if (!file.Exists)
                throw new FileNotFoundException(
                    $"The file {file.FullName} does not exist or cannot be accessed"
                );

            // Create a hash object
            using var hash = SHA256.Create();

            // Create a stream to read the file
            using var stream = file.OpenRead();

            // Compute the hash
            var hashBytes = hash.ComputeHash(stream);

            // Return the hash as a string
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}

namespace BackupUtility
{
    public static class Files
    {
        /// <summary>
        /// Get all files in a directory
        ///
        /// (if the path is a file, it will return an enumerable with only that file)
        /// </summary>
        /// <param name="path">The directory to get the files from</param>
        /// <returns>An enumerable of all files in the directory</returns>
        public static IEnumerable<FileInfo> GetAllFiles(string path)
        {
            // Normalize the path
            path = NormalizePath(path);

            // Check if the path is a file
            if (File.Exists(path))
                // Return a hashset with only that file
                return new FileInfo[] { new(path) };

            // If it's a directory, return all files in the directory
            return new DirectoryInfo(path).EnumerateFiles("*", SearchOption.AllDirectories);
        }

        /// <summary>
        /// Normalize a path
        /// </summary>
        /// <param name="path">The path to normalize</param>
        /// <returns>The normalized path</returns>
        public static string NormalizePath(string path)
        {
            // Handle `~` in the path
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (path == "~")
                return home;
            else if (path.StartsWith("~/"))
                return Path.Combine(home, path[2..]);

            // Return the full path
            return Path.GetFullPath(path);
        }
    }
}

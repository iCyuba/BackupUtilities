namespace BackupUtilities.Client;

public static class PathUtils
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
            return [new(path)];

        // If it's a directory, return all files in the directory
        if (Directory.Exists(path))
            return new DirectoryInfo(path).EnumerateFiles("*", SearchOption.AllDirectories);

        // If the path doesn't exist, return an empty enumerable
        return Array.Empty<FileInfo>();
    }

    /// <summary>
    /// Get all files in multiple directories.
    ///
    /// Overlapping files will be deduplicated.
    /// </summary>
    /// <param name="paths">The directories to get the files from</param>
    /// <returns>An enumerable of all files in the directories</returns>
    public static IEnumerable<FileInfo> GetAllFiles(IEnumerable<string> paths)
    {
        // Get all files from each path, flatten the result and deduplicate the files
        return paths.SelectMany(GetAllFiles).DistinctBy((file) => file.FullName);
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

    /// <summary>
    /// Get a path relative to the root of the filesystem
    /// </summary>
    /// <param name="path">The path to get the relative path for</param>
    /// <returns>The relative path</returns>
    public static string GetRelativePath(string path)
    {
        // Get the root of the filesystem
        string root = Path.GetPathRoot(path)!;

        // Get the relative path
        return Path.GetRelativePath(root, path);
    }

    public static string GetRelativePath(FileSystemInfo file) => GetRelativePath(file.FullName);
}

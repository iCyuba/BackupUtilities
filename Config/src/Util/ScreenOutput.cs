namespace BackupUtilities.Config.Util;

public static class ScreenOutput
{
    /// <summary>
    /// Expands the output to the specified width and height.
    /// </summary>
    /// <param name="output">The output to expand</param>
    /// <param name="width">The width to expand to</param>
    /// <param name="height">The height to expand to</param>
    /// <param name="offsets">The offsets of the existing output</param>
    /// <returns>The expanded output</returns>
    public static string[,] Expand(
        this string[,] output,
        int width,
        int height,
        (int x, int y) offsets = default
    )
    {
        // If the output is the desired size, return it as is
        if (output.GetLength(0) == height && output.GetLength(1) == width)
            return output;

        string[,] expanded = new string[height, width];
        expanded.Merge(output, offsets);

        return expanded;
    }

    /// <summary>
    /// Merges 1 output into another at the specified offsets.
    /// </summary>
    /// <param name="output">The output to merge into</param>
    /// <param name="other">The ouput to merge</param>
    /// <param name="offsets">Where to merge the output</param>
    public static void Merge(
        this string[,] output,
        string[,] other,
        (int x, int y) offsets = default
    )
    {
        int width = other.GetLength(1);
        int height = other.GetLength(0);

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            int xx = x + offsets.x;
            int yy = y + offsets.y;

            if (xx < 0 || yy < 0 || xx >= output.GetLength(1) || yy >= output.GetLength(0))
                continue;

            output[yy, xx] = other[y, x];
        }
    }
}

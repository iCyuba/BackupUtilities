namespace BackupUtilities.Config.Util;

public static class ScreenBuffer
{
    /// <summary>
    /// Expands the buffer to the specified width and height.
    /// </summary>
    /// <param name="buffer">The buffer to expand</param>
    /// <param name="width">The width to expand to</param>
    /// <param name="height">The height to expand to</param>
    /// <param name="offsets">The offsets of the existing buffer</param>
    /// <returns>The expanded buffer</returns>
    public static Character[,] Expand(
        this Character[,] buffer,
        int width,
        int height,
        (int x, int y) offsets = default
    )
    {
        // If the buffer is the desired size, return it as is
        if (buffer.GetLength(0) == height && buffer.GetLength(1) == width)
            return buffer;

        var expanded = new Character[height, width];
        expanded.Merge(buffer, offsets);

        return expanded;
    }

    /// <summary>
    /// Merges 1 buffer into another at the specified offsets.
    /// </summary>
    /// <param name="buffer">The buffer to merge into</param>
    /// <param name="other">The buffer to merge</param>
    /// <param name="offsets">Where to merge the buffer</param>
    public static void Merge(
        this Character[,] buffer,
        Character[,] other,
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

            if (xx < 0 || yy < 0 || xx >= buffer.GetLength(1) || yy >= buffer.GetLength(0))
                continue;

            buffer[yy, xx] = other[y, x];
        }
    }
}

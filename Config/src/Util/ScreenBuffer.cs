using System.Numerics;

namespace BackupUtilities.Config.Util;

public static class ScreenBuffer
{
    /// <summary>
    /// Expands the buffer to the specified width and height.
    /// </summary>
    /// <param name="buffer">The buffer to expand</param>
    /// <param name="size">The size to expand to</param>
    /// <param name="offsets">The offsets of the existing buffer</param>
    /// <returns>The expanded buffer</returns>
    public static Character[,] Expand(
        this Character[,] buffer,
        Vector2 size,
        Vector2 offsets = default
    )
    {
        // If the buffer is the desired size, return it as is
        if (buffer.GetLength(0) == size.Y && buffer.GetLength(1) == size.X)
            return buffer;

        var expanded = new Character[(int)size.Y, (int)size.X];
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
        Vector2 offsets = default
    )
    {
        int width = other.GetLength(1);
        int height = other.GetLength(0);

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            int xx = x + (int)offsets.X;
            int yy = y + (int)offsets.Y;

            if (xx < 0 || yy < 0 || xx >= buffer.GetLength(1) || yy >= buffer.GetLength(0))
                continue;

            buffer[yy, xx] = Character.MergeCharacters(buffer[yy, xx], other[y, x]);
        }
    }
}

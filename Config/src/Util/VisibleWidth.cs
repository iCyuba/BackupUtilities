using Wcwidth;

namespace BackupUtilities.Config.Util;

public static class VisibleWidth
{
    /// <summary>
    /// Get the visible width of a character
    /// </summary>
    /// <param name="c">The character to measure</param>
    /// <returns>The width of the character (-1, 0, 1, 2).</returns>
    public static int Width(this char c) => UnicodeCalculator.GetWidth(c);

    /// <summary>
    /// Get the visible width of a string
    /// </summary>
    /// <param name="str">The string to measure</param>
    /// <returns>The width of the string</returns>
    public static int Width(this string str)
    {
        int total = 0;
        foreach (char c in str)
        {
            // Using the wcwidth library to get the width of the character
            int width = c.Width();

            // We don't want control characters to subtract from the total width
            if (width > 0)
                total += width;
        }

        return total;
    }
}

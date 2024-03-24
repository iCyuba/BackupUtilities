using System.Globalization;

namespace BackupUtilities.Config.Util;

public static class Intl
{
    /// <summary>
    /// Formats a number to a string with an optional suffix (K, M, B)
    /// </summary>
    /// <param name="num">The number to format</param>
    /// <returns>A formatted string</returns>
    public static string FormatNumber(this int num)
    {
        int abs = int.Abs(num);

        return abs switch
        {
            >= 1_000_000_000 => num / 1_000_000_000 + "B",
            >= 1_000_000 => num / 1_000_000 + "M",
            >= 1_000 => num / 1_000 + "K",
            _ => num.ToString(CultureInfo.InvariantCulture)
        };
    }
}

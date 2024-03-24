using System.Text.RegularExpressions;

namespace BackupUtilities.Config.Util;

/// <summary>
/// RGBA color
/// </summary>
public partial record Color(byte R, byte G, byte B, byte A = 255)
{
    /// <summary>
    /// The target of the color. (Foreground or Background)
    /// </summary>
    public enum Target
    {
        Foreground = 38,
        Background = 48
    }

    public readonly struct Group
    {
        public Color Light { get; init; }
        public Color Regular { get; init; }
        public Color Dark { get; init; }
    }

    /// <summary>
    /// Whether to force 8-bit colors.
    /// </summary>
    private static readonly bool _force8Bit = Environment.GetCommandLineArgs().Contains("--8bit");

    public static Color White { get; } = FromHex("#fff");

    // Colors from https://tailwindcss.com/docs/customizing-colors
    public static Group Slate { get; } =
        new()
        {
            Light = FromHex("#94a3b8"),
            Regular = FromHex("#64748b"),
            Dark = FromHex("#475569")
        };

    public static Group Pink { get; } =
        new()
        {
            Light = FromHex("#f472b6"),
            Regular = FromHex("#ec4899"),
            Dark = FromHex("#db2777")
        };

    public static Group Emerald { get; } =
        new()
        {
            Light = FromHex("#34d399"),
            Regular = FromHex("#10b981"),
            Dark = FromHex("#059669")
        };

    public static Group Red { get; } =
        new()
        {
            Light = FromHex("#f87171"),
            Regular = FromHex("#ef4444"),
            Dark = FromHex("#dc2626")
        };

    public static Group Primary => Pink;

    [GeneratedRegex(@"^[0-9A-F]{3,4}$|^[0-9A-F]{6}$|^[0-9A-F]{8}$", RegexOptions.IgnoreCase)]
    private static partial Regex HexRegex();

    /// <summary>
    /// Parse a hex color string.
    /// </summary>
    /// <param name="hex">The hex color string.</param>
    /// <returns>The color.</returns>
    /// <exception cref="ArgumentException">Thrown when the hex color string is invalid.</exception>
    public static Color FromHex(string hex)
    {
        if (hex.StartsWith('#'))
            hex = hex[1..];

        Regex regex = HexRegex();
        if (!regex.IsMatch(hex))
            throw new ArgumentException("Invalid hex color format");

        if (hex.Length == 4 || hex.Length == 3)
            hex = string.Concat(hex.Select(c => $"{c}{c}"));

        if (hex.Length == 6)
            hex = $"{hex}FF";

        return new(
            Convert.ToByte(hex[..2], 16),
            Convert.ToByte(hex[2..4], 16),
            Convert.ToByte(hex[4..6], 16),
            Convert.ToByte(hex[6..], 16)
        );
    }

    /// <summary>
    /// Convert the color to a hex string.
    /// </summary>
    /// <returns>Hex color string.</returns>
    public override string ToString() => $"#{R:X2}{G:X2}{B:X2}{(A == 255 ? "" : $"{A:X2}")}";

    private byte To8Bit()
    {
        if (R == G && G == B)
            if (R == 0)
                return 16;
            else if (R == 255)
                return 255;
            else
                return (byte)(232 + R / 255f * 24);

        // 255 / 5 => 51
        return (byte)(16 + (R / 51 * 36) + (G / 51 * 6) + (B / 51));
    }

    private string To8BitANSIString(Target t) => $"\x1b[{(int)t};5;{To8Bit()}m";

    private string To24BitANSIString(Target t) => $"\x1b[{(int)t};2;{R};{G};{B}m";

    /// <summary>
    /// Convert the color to an ANSI escape sequence.
    /// </summary>
    /// <param name="t">The target of the color. (Foreground or Background)</param>
    /// <returns>An ANSI escape sequence.</returns>
    public string ToANSIString(Target t)
    {
        if (A < 255)
            return Mix(White, this).ToANSIString(t);

        return _force8Bit ? To8BitANSIString(t) : $"{To8BitANSIString(t)}{To24BitANSIString(t)}";
    }

    /// <summary>
    /// Mix two colors.
    /// </summary>
    /// <param name="bottom">Bottom color</param>
    /// <param name="top">Top color</param>
    /// <returns>A mix of the two.</returns>
    public static Color Mix(Color bottom, Color top)
    {
        byte alpha = top.A;

        byte r = (byte)((bottom.R * (255 - alpha) + top.R * alpha) / 255);
        byte g = (byte)((bottom.G * (255 - alpha) + top.G * alpha) / 255);
        byte b = (byte)((bottom.B * (255 - alpha) + top.B * alpha) / 255);

        return new(r, g, b, 255);
    }
}

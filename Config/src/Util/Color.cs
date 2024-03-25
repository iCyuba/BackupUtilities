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

    public record Group(Color Primary, Color Secondary);

    public record Group3(Color Primary, Color Secondary, Color Tertiary)
        : Group(Primary, Secondary);

    /// <summary>
    /// Whether to force 8-bit colors.
    /// </summary>
    private static readonly bool Force8Bit = Environment.GetCommandLineArgs().Contains("--8bit");

    /// <summary>
    /// Whether to use light mode.
    /// </summary>
    private static readonly bool LightMode =
        Environment.GetEnvironmentVariable("THEME")?.ToLowerInvariant() == "light";

    public static Color White { get; } = FromHex("#fff");

    // Colors from https://tailwindcss.com/docs/customizing-colors

    public static Group App { get; } =
        LightMode ? new(White, FromHex("#e2e8f0")) : new(FromHex("#111827"), FromHex("#1f2937"));

    public static Group3 Element { get; } =
        LightMode
            ? new(FromHex("#64748b"), FromHex("#94a3b8"), FromHex("#f1f5f9"))
            : new(FromHex("#4b5563"), FromHex("#374151"), FromHex("#1f2937"));

    public static Color Focus { get; } = FromHex(LightMode ? "#4b5563" : "#475569");

    public static Group Foreground { get; } =
        LightMode
            ? new(FromHex("#1e293b"), FromHex("#475569"))
            : new(FromHex("#e5e7eb"), FromHex("#d1d5db"));

    public static Color Overlay { get; } = FromHex(LightMode ? "#64748b40" : "#37415140");

    public static Group Pink { get; } =
        new(FromHex("#ec4899"), FromHex(LightMode ? "#f472b6" : "#db2777"));
    public static Group Red { get; } =
        new(FromHex("#ef4444"), FromHex(LightMode ? "#f87171" : "#dc2626"));
    public static Group Green { get; } =
        new(FromHex("#10b981"), FromHex(LightMode ? "#34d399" : "#059669"));
    public static Group Yellow { get; } =
        new(FromHex("#f59e0b"), FromHex(LightMode ? "#fbbf24" : "#d97706"));
    public static Group Blue { get; } =
        new(FromHex("#0ea5e9"), FromHex(LightMode ? "#60a5fa" : "#0284c7"));

    public static Group Primary =>
        Environment.GetEnvironmentVariable("PRIMARY_COLOR") switch
        {
            "red" => Red,
            "green" => Green,
            "yellow" => Yellow,
            "blue" => Blue,
            _ => Pink
        };

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
            return Mix(App.Primary, this).ToANSIString(t);

        return Force8Bit ? To8BitANSIString(t) : $"{To8BitANSIString(t)}{To24BitANSIString(t)}";
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

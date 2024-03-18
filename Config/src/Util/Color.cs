using System.Text.RegularExpressions;

namespace BackupUtilities.Config.Util;

public partial record Color(byte R, byte G, byte B)
{
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

    private static bool _force8Bit = Environment.GetCommandLineArgs().Contains("--8bit");

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

    public static Group Primary => Pink;

    [GeneratedRegex(@"^[0-9A-F]{3}$|^[0-9A-F]{6}$", RegexOptions.IgnoreCase)]
    private static partial Regex HexRegex();

    public static Color FromHex(string hex)
    {
        if (hex.StartsWith('#'))
            hex = hex[1..];

        Regex regex = HexRegex();
        if (!regex.IsMatch(hex))
            throw new ArgumentException("Invalid hex color format");

        if (hex.Length == 3)
            hex = string.Concat(hex.Select(c => $"{c}{c}"));

        return new(
            Convert.ToByte(hex[..2], 16),
            Convert.ToByte(hex[2..4], 16),
            Convert.ToByte(hex[4..], 16)
        );
    }

    public override string ToString() => $"#{R:X2}{G:X2}{B:X2}";

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

    public string ToANSIString(Target t) =>
        _force8Bit ? To8BitANSIString(t) : $"{To8BitANSIString(t)}{To24BitANSIString(t)}";
}

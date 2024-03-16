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

    public static Color White { get; } = FromHex("#fff");

    // Colors from https://tailwindcss.com/docs/customizing-colors
    public static Group Pink { get; } =
        new()
        {
            Light = FromHex("#f472b6"),
            Regular = FromHex("#ec4899"),
            Dark = FromHex("#db2777")
        };

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

    public string ToANSIString(Target target) => $"\x1b[{(int)target};2;{R};{G};{B}m";
}

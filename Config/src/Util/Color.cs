using System.Text.RegularExpressions;

namespace BackupUtilities.Config.Util;

public readonly partial struct Color(byte r, byte g, byte b)
{
    public enum Target
    {
        Foreground = 38,
        Background = 48
    }

    [GeneratedRegex(@"^[0-9A-F]{3}$|^[0-9A-F]{6}$", RegexOptions.IgnoreCase)]
    private static partial Regex HexRegex();

    public byte R { get; } = r;
    public byte G { get; } = g;
    public byte B { get; } = b;

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

    public override readonly string ToString() => $"#{R:X2}{G:X2}{B:X2}";

    public readonly string ToANSI(Target target) => $"\x1b[{(int)target};2;{R};{G};{B}m";
}

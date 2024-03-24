namespace BackupUtilities.Config.Util;

/// <summary>
/// Represents a single character in a console.
/// </summary>
/// <param name="value"></param>
public struct Character(string value = "")
{
    public string Value { get; set; } = value;

    public Color? Foreground { get; set; }
    public Color? Background { get; set; }
    public bool Bold { get; set; }
    public bool Italic { get; set; }
    public bool Underline { get; set; }
    public bool Strikethrough { get; set; }

    private readonly string ForegroundAnsi =>
        Foreground?.ToANSIString(Color.Target.Foreground) ?? "\x1b[39m";

    private readonly string BackgroundAnsi =>
        Background?.ToANSIString(Color.Target.Background) ?? "\x1b[49m";

    private readonly string BoldAnsi => $"\x1b[{(Bold ? 1 : 22)}m";
    private readonly string ItalicAnsi => $"\x1b[{(Italic ? 3 : 23)}m";
    private readonly string UnderlineAnsi => $"\x1b[{(Underline ? 4 : 24)}m";
    private readonly string StrikethroughAnsi => $"\x1b[{(Strikethrough ? 9 : 29)}m";

    public override readonly string ToString() => Value ?? " ";

    public readonly string ToANSIString(Character? previous)
    {
        string ansi = "";

        if (Foreground != previous?.Foreground)
            ansi += ForegroundAnsi;

        if (Background != previous?.Background)
            ansi += BackgroundAnsi;

        if (Bold != previous?.Bold)
            ansi += BoldAnsi;

        if (Italic != previous?.Italic)
            ansi += ItalicAnsi;

        if (Underline != previous?.Underline)
            ansi += UnderlineAnsi;

        if (Strikethrough != previous?.Strikethrough)
            ansi += StrikethroughAnsi;

        return ansi + ToString();
    }

    public static Character MergeCharacters(Character bottom, Character top)
    {
        if (
            top.Value is null or " "
            && top is { Underline: false, Strikethrough: false }
            && top.Background?.A != 255
        )
        {
            top.Value = bottom.Value;
            top.Bold = bottom.Bold;
            top.Italic = bottom.Italic;
            top.Underline = bottom.Underline;
            top.Strikethrough = bottom.Strikethrough;
        }

        if (top.Background?.A < 255)
        {
            if (bottom.Foreground != null)
                top.Foreground = Color.Mix(bottom.Foreground ?? Color.White, top.Foreground!);

            if (bottom.Background != null)
                top.Background = Color.Mix(bottom.Background ?? Color.White, top.Background!);

            return top;
        }

        top.Foreground ??= bottom.Foreground;
        top.Background ??= bottom.Background;

        return top;
    }
}

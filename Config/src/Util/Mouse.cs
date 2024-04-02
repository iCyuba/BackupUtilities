using System.Numerics;

namespace BackupUtilities.Config.Util;

/// <summary>
/// Info about mouse input.<br/>
/// <br/>
/// See: https://invisible-island.net/xterm/ctlseqs/ctlseqs.html#h2-Mouse-Tracking
/// </summary>
public class Mouse
{
    public enum MouseButton
    {
        Left,
        Right,
        Middle,
        None
    }

    [Flags]
    public enum MouseModifiers
    {
        Shift = 4,
        Meta = 8,
        Control = 16,
        Motion = 32,
        Scroll = 64 // Mouse 1 -> Up, Mouse 2 -> Down
    }

    public Vector2 Position { get; }

    public MouseButton Button { get; }
    public MouseModifiers Modifiers { get; }

    public bool Released { get; }
    public bool Scroll => Modifiers.HasFlag(MouseModifiers.Scroll);
    public bool Motion => Modifiers.HasFlag(MouseModifiers.Motion);

    private Mouse(Vector2 position, MouseButton button, MouseModifiers modifiers, bool released)
    {
        Position = position;
        Button = button;
        Modifiers = modifiers;
        Released = released;
    }

    /// <summary>
    /// Parse a mouse event using the SGR format (1006).
    /// </summary>
    /// <param name="sgr">The SGR string to parse.</param>
    public static Mouse ParseSGR(string sgr)
    {
        if (!sgr.StartsWith("\x1b[<") || !sgr.ToLowerInvariant().EndsWith('m'))
            throw new ArgumentException("Invalid SGR format");

        bool released = sgr[^1] == 'm';

        string[] parts = sgr[3..^1].Split(';');

        if (parts.Length < 3)
            throw new ArgumentException("Invalid SGR format");

        int value = int.Parse(parts[0]);
        var button = (MouseButton)(value & 0b11);
        var modifier = (MouseModifiers)(value & 0b11111100);

        int x = int.Parse(parts[1]);
        int y = int.Parse(parts[2]);

        return new(new(x, y), button, modifier, released);
    }
}

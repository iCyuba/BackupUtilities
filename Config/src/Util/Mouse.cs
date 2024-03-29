using System.Numerics;

namespace BackupUtilities.Config.Util;

/// <summary>
/// Info about mouse input.<br/>
/// <br/>
/// See: https://invisible-island.net/xterm/ctlseqs/ctlseqs.html#h2-Mouse-Tracking
/// </summary>
public class Mouse
{
    public enum MouseState
    {
        Pressed,
        Released
    }

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
        Control = 16
    }

    public Vector2 Position { get; private set; }

    public MouseButton Button { get; private set; }
    public MouseModifiers Modifiers { get; private set; }

    public MouseState State { get; private set; }

    private Mouse(Vector2 position, MouseButton button, MouseModifiers modifiers, MouseState state)
    {
        Position = position;
        Button = button;
        Modifiers = modifiers;
        State = state;
    }

    /// <summary>
    /// Parse a mouse event using the SGR format (1006).
    /// </summary>
    /// <param name="sgr">The SGR string to parse.</param>
    public static Mouse ParseSGR(string sgr)
    {
        if (!sgr.StartsWith("\x1b[<") || !sgr.ToLowerInvariant().EndsWith('m'))
            throw new ArgumentException("Invalid SGR format");

        var state = sgr[^1] == 'm' ? MouseState.Released : MouseState.Pressed;

        string[] parts = sgr[3..^1].Split(';');

        if (parts.Length < 3)
            throw new ArgumentException("Invalid SGR format");

        int value = int.Parse(parts[0]);
        var button = (MouseButton)(value & 0b111);
        var modifier = (MouseModifiers)(value & 0b111000);

        int x = int.Parse(parts[1]);
        int y = int.Parse(parts[2]);

        return new(new(x, y), button, modifier, state);
    }
}

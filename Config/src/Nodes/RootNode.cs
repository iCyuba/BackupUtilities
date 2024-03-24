using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Nodes;

/// <summary>
/// The root node of the layout tree.
/// </summary>
public class RootNode : RenderableNode
{
    // Readonly properties for the root node
    public new static PositionType PositionType => PositionType.Relative;

    public RootNode()
        : base()
    {
        // Set position to relative (if this wasn't set, absolutely positioned nodes would not render at all)
        base.PositionType = PositionType.Relative;
    }

    /// <summary>
    /// Print the layout to the console.
    /// </summary>
    public void Print()
    {
        int width = Console.WindowWidth;
        int height = Console.WindowHeight;

        // If the layout is dirty, recalculate it
        if (IsDirty)
            CalculateLayout(width, height);

        var render = Render();
        var buffer = render.Normal.Buffer.Expand(new(width, height), -render.Normal.Offsets);

        Console.SetCursorPosition(0, 0);

        Character? last = null;
        foreach (var character in buffer)
        {
            Console.Write(character.ToANSIString(last));
            last = character;
        }

        Console.ResetColor();
    }
}

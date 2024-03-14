using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Nodes;

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

    public void Print()
    {
        int width = Console.WindowWidth;
        int height = Console.WindowHeight;

        // If the layout is dirty, recalculate it
        if (IsDirty)
            CalculateLayout(width, height);

        var render = Render();
        string[,] output = render
            .Buffer
            .Expand(width, height, (-render.Offsets.x, -render.Offsets.y));

        Console.SetCursorPosition(0, 0);

        foreach (var character in output)
            Console.Write(character ?? " ");
    }
}

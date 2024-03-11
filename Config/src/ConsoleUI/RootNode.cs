using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.ConsoleUI;

public class RootNode : RenderableNode
{
    // Readonly properties for the root node
    public static new PositionType PositionType => PositionType.Relative;

    public RootNode()
        : base()
    {
        // Set position to relative
        base.PositionType = PositionType.Relative;
    }

    public void Print()
    {
        int width = Console.WindowWidth;
        int height = Console.WindowHeight;

        CalculateLayout(width, height);

        var render = Render();
        string[,] output = render
            .Output
            .Expand(width, height, (-render.Offsets.x, -render.Offsets.y));

        Console.SetCursorPosition(0, 0);

        foreach (var character in output)
            Console.Write(character ?? " ");
    }
}

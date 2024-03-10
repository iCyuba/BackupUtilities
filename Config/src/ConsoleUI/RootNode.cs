using BackupUtilities.Config.Yoga.Interop;

namespace BackupUtilities.Config.ConsoleUI;

public class RootNode : RenderableNode
{
    // Readonly properties for the root node
    public static new YGPositionType PositionType => YGPositionType.YGPositionTypeRelative;

    public RootNode()
    {
        // Set position to relative
        base.PositionType = YGPositionType.YGPositionTypeRelative;
    }

    public override string[,] Render()
    {
        throw new NotImplementedException();
    }

    public void Print()
    {
        int width = Console.WindowWidth;
        int height = Console.WindowHeight;

        CalculateLayout(width, height);

        if (GetChild(0) is not RenderableNode renderableNode)
            throw new InvalidOperationException("Root node must have a renderable child");

        string[,] screenBuffer = renderableNode.Render();

        // Expand the buffer to the size of the console
        if (screenBuffer.GetLength(0) < height || screenBuffer.GetLength(1) < width)
        {
            string[,] newBuffer = new string[height, width];

            for (int x = 0; x < screenBuffer.GetLength(1); x++)
            for (int y = 0; y < screenBuffer.GetLength(0); y++)
                newBuffer[y, x] = screenBuffer[y, x];

            screenBuffer = newBuffer;
        }

        Console.SetCursorPosition(0, 0);

        foreach (var character in screenBuffer)
            Console.Write(character ?? " ");
    }
}

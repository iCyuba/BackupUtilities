using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config;

public class ConsoleRenderer(Node root)
{
    private string[,]? _screenBuffer;
    private readonly Node _root = root;

    public void Render()
    {
        int width = Console.WindowWidth;
        int height = Console.WindowHeight;

        _screenBuffer = new string[height, width];

        _root.CalculateLayout(width, height);
        RenderNode(_root);

        Console.SetCursorPosition(0, 0);
        foreach (var character in _screenBuffer)
            Console.Write(character ?? " ");
    }

    private void RenderNode(Node node)
    {
        float startX = node.ComputedLeft;
        float endX = startX + node.ComputedWidth;

        float startY = node.ComputedTop;
        float endY = startY + node.ComputedHeight;

        // Border
        for (int x = (int)startX; x < endX; x++)
        for (int y = (int)startY; y < endY; y++)
            if (x == startX || x == endX - 1 || y == startY || y == endY - 1)
                _screenBuffer![y, x] = "█";

        // Children
        for (nuint i = 0; i < node.ChildCount; i++)
            RenderNode(node.GetChild((uint)i)!);
    }
}

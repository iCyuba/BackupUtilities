using System.Runtime.InteropServices;
using BackupUtilities.Config.ConsoleUI;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config;

internal static class Program
{
    private static void Main()
    {
        Console.Clear();

        RootNode root = new() { FlexWrap = Wrap.Wrap, Overflow = Overflow.Scroll };
        root.SetGap(Gutter.All, 1);

        // Add 5k nodes to the root node
        for (int i = 0; i < 5000; i++)
            root.InsertChild(new FancyNode() { Width = new(10), Height = new(i % 25 + 2) }, i);

        // Add the modal
        var modal = new FancyNode()
        {
            PositionType = PositionType.Absolute,
            IsFixed = true,
            AlignItems = Align.Center,
            JustifyContent = Justify.Center
        };
        modal.SetPosition(Edge.All, 10);

        var modalText = new TextNode("This is my amazing modal")
        {
            Width = new(15),
            Color = Color.FromHex("#c50282"),
            Bold = true,
            Italic = true,
            Underline = true,
            Strikethrough = true
        };

        modal.InsertChild(modalText, 0);
        root.InsertChild(modal, 0);

        // Disable cursor
        Console.CursorVisible = false;

        // Print the root node
        root.Print();
        Console.ReadKey(true);

        // This doesn't work on Windows, I'm sorry to all 0 of my Windows users
        PosixSignalRegistration.Create(
            PosixSignal.SIGWINCH,
            _ =>
            {
                root.CalculateLayout(Console.WindowWidth, Console.WindowHeight);
                root.Print();
            }
        );

        // Scrolllllll
        while (true)
        {
            root.ScrollOffsets = new(0, root.ScrollOffsets.y + 1);

            // This is a workaround, because if a layout happens on another thread,
            // this may throw an index out of range exception
            try
            {
                root.Print();
            }
            catch { }

            // Thread.Sleep(50);
        }

        // Freeing the nodes isn't necessary, but why not
        root.FreeRecursive();
    }
}

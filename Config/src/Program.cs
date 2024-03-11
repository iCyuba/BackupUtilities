using BackupUtilities.Config.ConsoleUI;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config;

internal static class Program
{
    private static void Main()
    {
        Console.Clear();

        RootNode root = new() { JustifyContent = Justify.SpaceEvenly, AlignItems = Align.Center };
        FancyNode node = new() { Width = new(Unit.Percent, 50), Height = new(Unit.Percent, 50) };
        FancyNode node2 = new() { Width = new(Unit.Percent, 50), Height = new(Unit.Percent, 50) };

        node2.SetMargin(Edge.Top, 17);
        node2.SetMargin(Edge.Left, -15);

        root.InsertChild(node, 0);
        node.InsertChild(node2, 0);

        TextNode text =
            new(
                "Hello, World! Lorem ipsum dolor sit amet, consectetur adipiscing elit.\n Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
            )
            {
                Width = new(Unit.Percent, 35)
            };
        text.SetMargin(Edge.Top, -5);
        text.SetMargin(Edge.Left, 65);
        text.SetMargin(Edge.Right, -65);

        TextNode text2 =
            new("Hello, World! Lorem ipsum dolor sit amet, consectetur adipiscing elit.");

        text2.SetMargin(Edge.Top, -3);
        text2.SetMargin(Edge.Left, -25);
        text2.SetMargin(Edge.Right, 25);

        node.InsertChild(text, 1);
        node2.InsertChild(text2, 0);

        // Print the root node
        root.Print();

        // Freeing the nodes isn't necessary, but why not
        root.FreeRecursive();
    }
}

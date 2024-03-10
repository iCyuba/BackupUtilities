using BackupUtilities.Config.ConsoleUI;
using BackupUtilities.Config.Yoga;
using BackupUtilities.Config.Yoga.Interop;

namespace BackupUtilities.Config;

internal static class Program
{
    private static void Main()
    {
        RootNode root = new();

        TextNode text =
            new(
                "Hello, World! Lorem ipsum dolor sit amet, consectetur adipiscing elit.\n Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
            );

        text.SetWidthPercent(25);
        root.InsertChild(text, 0);

        // Print the root node
        root.Print();

        // Freeing the nodes isn't necessary, but why not
        root.FreeRecursive();
    }
}

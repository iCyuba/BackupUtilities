using BackupUtilities.Config.Components.Windows;

namespace BackupUtilities.Config;

internal static class Program
{
    private static void Main()
    {
        App app = new();

        MainWindow test = new(app);
        app.SetWindow(test);

        app.Run();

        // Clear the console before exiting
        Console.Clear();
    }
}

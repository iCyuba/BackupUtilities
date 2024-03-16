using BackupUtilities.Config.Components.Views;

namespace BackupUtilities.Config;

internal static class Program
{
    private static void Main()
    {
        App app = new();

        MainView test = new();
        app.SetView(test);

        app.Run();

        // Clear the console before exiting
        Console.Clear();
    }
}

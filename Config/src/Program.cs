using BackupUtilities.Config.Views;

namespace BackupUtilities.Config;

internal static class Program
{
    private static void Main()
    {
        App app = new();

        MainView test = new(app);
        app.SetView(test);

        app.Run();

        // Clear the console before exiting
        Console.Clear();
    }
}

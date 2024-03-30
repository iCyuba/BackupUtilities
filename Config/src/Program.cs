using BackupUtilities.Config.Components.Windows;

namespace BackupUtilities.Config;

internal static class Program
{
    private static void Main()
    {
        App app = new();

        MainWindow main = new(app);
        app.SetWindow(main);

        app.Run();
    }
}

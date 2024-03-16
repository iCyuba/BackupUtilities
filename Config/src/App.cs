using System.Runtime.InteropServices;
using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;

namespace BackupUtilities.Config;

public class App
{
    private readonly RootNode _root = new();
    private readonly Stack<IView> _views = [];
    private bool _running;

    public void Run()
    {
        if (_running)
            throw new InvalidOperationException("App is already running");

        _running = true;

        Console.Title = "Backup Utilities: Config Editor";
        Console.ResetColor();
        Console.CursorVisible = false;
        Console.Clear();

        // Handle screen resizing
        // This doesn't work on Windows, I'm sorry to all 0 of my Windows users
#pragma warning disable CA1416
        var resizeRegistration = PosixSignalRegistration.Create(
            PosixSignal.SIGWINCH,
            _ =>
            {
                _root.CalculateLayout(Console.WindowWidth, Console.WindowHeight);
                _root.Print();
            }
        );
#pragma warning restore CA1416

        while (_views.TryPeek(out var view))
        {
            // This is a workaround, because if a layout happens on another thread,
            // this may throw an index out of range exception
            try
            {
                _root.Print();
            }
            catch { }

            var key = Console.ReadKey(true);
            view.HandleInput(key);
        }

        resizeRegistration.Dispose();
        _running = false;
    }

    public void SetView(IView view)
    {
        _views.Push(view);

        view.Closed += CloseView;
        _root.SetChildren([view.Node]);
    }

    private void CloseView()
    {
        if (_views.TryPop(out var view))
            view.Closed -= CloseView;
    }
}

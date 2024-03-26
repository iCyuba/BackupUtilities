using System.Runtime.InteropServices;
using System.Text;
using BackupUtilities.Config.Components.Windows;
using BackupUtilities.Config.Nodes;

namespace BackupUtilities.Config;

public class App
{
    public event Action? WindowChange;

    private readonly RootNode _root = new();
    private readonly Stack<IWindow> _windows = [];
    private bool _running;

    public IWindow? Window => _windows.Count > 0 ? _windows.Peek() : null;

    public void Run()
    {
        if (_running)
            throw new InvalidOperationException("App is already running");

        _running = true;

        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = "Backup Utilities: Config Editor";
        Console.CursorVisible = false;
        Console.Clear();

        // Handle screen resizing
        // This doesn't work on Windows, I'm sorry to all 0 of my Windows users

        PosixSignalRegistration? resizeRegistration = null;
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            resizeRegistration = PosixSignalRegistration.Create(
                PosixSignal.SIGWINCH,
                _ =>
                {
                    _root.CalculateLayout(Console.WindowWidth, Console.WindowHeight);
                    _root.Print();
                }
            );

        while (_windows.TryPeek(out var window))
        {
            // This is a workaround, because if a layout happens on another thread,
            // this may throw an index out of range exception
            try
            {
                // If a key is available, don't print.
                // This results in better drag and drop + paste performance
                if (!Console.KeyAvailable)
                    _root.Print();
            }
            catch { }

            var key = Console.ReadKey(true);
            window.HandleInput(key);
        }

        resizeRegistration?.Dispose();
        _running = false;
    }

    public void SetWindow(IWindow window)
    {
        _windows.Push(window);

        window.Closed += CloseWindow;
        _root.SetChildren([window.Node]);

        WindowChange?.Invoke();
    }

    private void CloseWindow()
    {
        if (_windows.TryPop(out var window))
            window.Closed -= CloseWindow;

        if (_windows.TryPeek(out var current))
            _root.SetChildren([current.Node]);

        WindowChange?.Invoke();
    }
}

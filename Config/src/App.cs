using System.Runtime.InteropServices;
using System.Text;
using BackupUtilities.Config.Components.Windows;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;

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

        Console.Write("\x1b[?1003h\x1b[?1006h"); // Enable mouse reporting

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

            List<ConsoleKeyInfo> keys = [];

            while (Console.KeyAvailable)
                keys.Add(Console.ReadKey(true));

            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i].Key == ConsoleKey.Escape)
                {
                    // Try to parse the mouse event, if it fails, just pass the keys to the window as input
                    try
                    {
                        string sgr = "";

                        // Keep adding to the sgr string until M is encountered.
                        // If there's no M, this will throw, and handle the input as normal
                        int j = i;
                        for (; j < keys.Count; j++)
                        {
                            sgr += keys[j].KeyChar;

                            if (keys[j].Key == ConsoleKey.M)
                                break;
                        }

                        // The parse will throw if it's not a valid mouse event
                        window.HandleMouse(Mouse.ParseSGR(sgr));

                        // Jump to the next char outside the mouse input
                        // (this is here so there's no jump if an exception is thrown)
                        i = j;
                        continue;
                    }
                    catch { }
                }

                window.HandleInput(keys[i]);
            }
        }

        resizeRegistration?.Dispose();
        _running = false;

        // Reset the console
        Console.CursorVisible = true;
        Console.Clear();
        Console.ResetColor();
        Console.Title = "";
        Console.Write("\x1b[?1003l\x1b[?1006l"); // Disable mouse reporting
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

using BackupUtilities.Config.Util;

namespace BackupUtilities.Config.Components;

/// <summary>
/// A component that can be interacted with.
/// </summary>
public interface IInteractive : IComponent
{
    public bool IsFocused { get; }
    public bool CapturesInput { get; }

    void HandleInput(ConsoleKeyInfo key);
    void HandleMouse(Mouse mouse);
}

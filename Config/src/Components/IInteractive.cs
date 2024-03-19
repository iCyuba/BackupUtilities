namespace BackupUtilities.Config.Components;

public interface IInteractive : IComponent
{
    public bool IsFocused { get; }
    public bool CapturesInput { get; }

    void HandleInput(ConsoleKeyInfo key);
}

namespace BackupUtilities.Config.Components;

public interface IInteractive : IComponent
{
    public bool IsFocused { get; }

    void HandleInput(ConsoleKeyInfo key);
}

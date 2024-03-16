namespace BackupUtilities.Config.Components;

public interface IInteractive : IComponent
{
    public bool IsFocused { get; set; }

    void HandleInput(ConsoleKeyInfo key);
}

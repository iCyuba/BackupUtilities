namespace BackupUtilities.Config.Components.Views;

public interface IView : IInteractive
{
    event Action? Closed;

    IComponent? Focus { get; }
}

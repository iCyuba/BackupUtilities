using BackupUtilities.Config.Nodes;

namespace BackupUtilities.Config.Views;

public interface IView
{
    event EventHandler? Closed;

    public RenderableNode Node { get; }

    void HandleInput(ConsoleKeyInfo key);
}

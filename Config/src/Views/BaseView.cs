using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Views;

public abstract class BaseView(App app) : IView
{
    public event EventHandler? Closed;

    protected readonly App _app = app;

    protected FancyNode Node { get; } =
        new()
        {
            FlexDirection = FlexDirection.Column,
            FlexGrow = 1,
            Overflow = Overflow.Scroll
        };

    RenderableNode IView.Node => Node;

    public abstract void HandleInput(ConsoleKeyInfo key);

    protected void Close() => Closed?.Invoke(this, EventArgs.Empty);
}

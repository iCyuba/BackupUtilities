using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Windows;

public abstract class BaseWindow : BaseView, IWindow
{
    public event Action? Close;

    protected readonly WindowTitle Title = new();
    private readonly FancyNode _container =
        new() { FlexDirection = FlexDirection.Column, FlexGrow = 1 };

    public override RenderableNode Node => _container;

    protected FancyNode Content { get; } = new() { FlexGrow = 1 };

    protected readonly App App;
    public override bool IsFocused => App.Window == this;

    protected BaseWindow(App app)
    {
        App = app;
        app.WindowChange += OnFocusChange;

        _container.SetChildren([Title.Node, Content]);
    }

    protected void OnClose() => Close?.Invoke();

    public override void HandleInput(ConsoleKeyInfo key)
    {
        if (key.Key != ConsoleKey.Tab)
        {
            base.HandleInput(key);
            return;
        }

        if (key.Modifiers.HasFlag(ConsoleModifiers.Shift))
            FocusPrevious();
        else
            FocusNext();
    }
}

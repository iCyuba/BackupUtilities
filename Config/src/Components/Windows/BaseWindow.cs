using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Windows;

/// <summary>
/// Base class for windows.
/// </summary>
public abstract class BaseWindow : TabView, IWindow
{
    public event Action? Closed;

    protected readonly WindowTitle Title = new();
    private readonly FancyNode _container =
        new() { FlexDirection = FlexDirection.Column, FlexGrow = 1 };

    public override RenderableNode Node => _container;

    protected readonly FancyNode Content = new() { FlexGrow = 1 };

    protected readonly App App;
    public override bool IsFocused => App.Window == this;

    protected BaseWindow(App app)
    {
        App = app;
        app.WindowChange += OnFocusChange;

        _container.SetPadding(Edge.All, 1);
        _container.SetGap(Gutter.All, 1);
        _container.SetChildren([Title.Node, Content]);
    }

    protected void OnClose() => Closed?.Invoke();
}

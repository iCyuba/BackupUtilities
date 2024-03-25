using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Windows;

/// <summary>
/// Base class for windows.
/// </summary>
public abstract class BaseWindow : TabView, IWindow
{
    public event Action? Closed;

    protected string Icon
    {
        get => _windowIcon.Text;
        set => _windowIcon.Text = value;
    }

    protected string Title
    {
        get => _windowName.Text;
        set => _windowName.Text = value;
    }

    private readonly Label _label;
    private readonly Label.TextContent _windowName = new("") { Bold = true };
    private readonly Label.TextContent _windowIcon =
        new("") { BackgroundColor = Color.Primary.Secondary };

    private readonly Label.Content _separator = new() { Style = Label.Content.ContentStyle.None };

    private readonly Label.TextContent _brandingName = new("Backup Utilities") { Bold = true };
    private readonly Label.TextContent _brandingIcon =
        new("") { BackgroundColor = Color.Primary.Secondary };

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

        _label = new([_windowIcon, _windowName, _separator, _brandingIcon, _brandingName]);
        _label.Register(this);

        _container.SetPadding(Edge.All, 1);
        _container.SetGap(Gutter.All, 1);
        _container.SetChildren([_label.Node, Content]);
    }

    protected void Close() => Closed?.Invoke();
}

using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Windows;

public abstract class BaseWindow : BaseView, IWindow
{
    public event Action? Close;

    private readonly FancyNode _container =
        new() { FlexDirection = FlexDirection.Column, FlexGrow = 1 };

    public override RenderableNode Node => _container;

    protected FancyNode Content { get; } = new() { FlexGrow = 1 };

    private readonly App _app;
    public override bool IsFocused => _app.Window == this;

    protected BaseWindow(string name, string icon, App app)
    {
        _app = app;
        app.WindowChange += OnFocusChange;

        Label label = new();

        Label.TextContent windowName = new(name) { Bold = true };
        Label.TextContent windowIcon = new(icon) { BackgroundColor = Color.Primary.Light };

        Label.Content separator = new() { Style = Label.Content.ContentStyle.None };

        Label.TextContent brandingName = new("Backup Utilities") { Bold = true };
        Label.TextContent brandingIcon = new("") { BackgroundColor = Color.Primary.Light };

        label.Children = [windowIcon, windowName, separator, brandingIcon, brandingName];
        label.Node.SetMargin(Edge.All, 1);

        _container.SetChildren([label.Node, Content]);

        label.Register(this);
        windowIcon.Register(this);
        windowName.Register(this);
        separator.Register(this);
        brandingIcon.Register(this);
        brandingName.Register(this);
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

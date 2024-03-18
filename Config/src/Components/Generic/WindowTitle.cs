using BackupUtilities.Config.Components.Base;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Generic;

public class WindowTitle : BaseComponent
{
    public string Text
    {
        get => _windowName.Text;
        set => _windowName.Text = value;
    }

    public string Icon
    {
        get => _windowIcon.Text;
        set => _windowIcon.Text = value;
    }

    protected override IEnumerable<IComponent> SubComponents =>
        [_windowIcon, _windowName, _separator, _brandingIcon, _brandingName];

    private readonly Label _label = new();
    public override RenderableNode Node => _label.Node;

    private readonly Label.TextContent _windowName = new("") { Bold = true };
    private readonly Label.TextContent _windowIcon =
        new("") { BackgroundColor = Color.Primary.Light };

    private readonly Label.Content _separator = new() { Style = Label.Content.ContentStyle.None };

    private readonly Label.TextContent _brandingName = new("Backup Utilities") { Bold = true };
    private readonly Label.TextContent _brandingIcon =
        new("") { BackgroundColor = Color.Primary.Light };

    public WindowTitle()
    {
        _label.Children = [_windowIcon, _windowName, _separator, _brandingIcon, _brandingName];
        _label.Node.SetMargin(Edge.All, 1);
    }
}

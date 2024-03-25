using BackupUtilities.Config.Components.Base;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Generic;

/// <summary>
/// Simple title component, used by modals.
/// </summary>
public sealed class Title : BaseComponent
{
    protected override IEnumerable<IComponent> SubComponents => [_label];

    public string Text
    {
        get => _text.Text;
        set => _text.Text = value;
    }

    public string Icon
    {
        get => _icon.Text;
        set => _icon.Text = value;
    }

    private readonly Label.TextContent _icon = new("");
    private readonly Label.TextContent _text = new("");
    private readonly Label _label;

    public override RenderableNode Node => _label.Node;

    public Title()
    {
        _label = new([_icon, _text]);
        _label.Node.SetMarginAuto(Edge.Horizontal);

        _icon.BackgroundColor = Color.Element.Primary;
        _text.BackgroundColor = Color.Element.Secondary;
    }
}

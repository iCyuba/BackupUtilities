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
    protected override IEnumerable<IComponent> SubComponents => [_label, _icon, _text];

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

    public Color.Group Color { get; init; } = Util.Color.Slate;

    private readonly Label.TextContent _icon = new("");
    private readonly Label.TextContent _text = new("");
    private readonly Label _label = new();

    public override RenderableNode Node => _label.Node;

    public Title()
    {
        _label.Node.SetMarginAuto(Edge.Horizontal);
        _label.Children = [_icon, _text];

        UpdateStyle();
    }

    protected override void UpdateStyle()
    {
        _icon.BackgroundColor = Color.Regular;
        _text.BackgroundColor = Color.Light;
    }
}
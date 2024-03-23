using BackupUtilities.Config.Components.Base;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;

namespace BackupUtilities.Config.Components.Generic;

public sealed class Button : BaseButton
{
    private Color.Group _color = Util.Color.Primary;
    public Color.Group Color
    {
        get => _color;
        set
        {
            _color = value;
            UpdateStyle();
        }
    }

    protected override IEnumerable<IComponent> SubComponents =>
        _text == null ? [_label, _icon] : [_label, _icon, _text];

    private readonly Label _label;
    private readonly Label.TextContent _icon;
    private readonly Label.TextContent? _text;

    public override RenderableNode Node => _label.Node;

    public Button(string icon, string? text = null)
    {
        _icon = new(icon);
        if (text != null)
            _text = new(text);

        _label = new() { Children = _text == null ? [_icon] : [_icon, _text], Gap = true };

        UpdateStyle();
    }

    protected override void UpdateStyle()
    {
        if (_text != null)
        {
            _text.Bold = IsFocused;
            _text.BackgroundColor = IsFocused ? Color.Dark : Color.Regular;
        }

        _icon.BackgroundColor = IsFocused ? Color.Regular : Color.Light;
    }
}

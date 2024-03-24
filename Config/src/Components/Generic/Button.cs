using BackupUtilities.Config.Components.Base;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;

namespace BackupUtilities.Config.Components.Generic;

/// <summary>
/// Generic button that can be clicked with either enter or spacebar.
/// </summary>
public sealed class Button : BaseButton
{
    private Color.Group _accent = Color.Primary;
    public Color.Group Accent
    {
        get => _accent;
        set
        {
            _accent = value;
            UpdateStyle();
        }
    }

    private bool _disabled;
    public bool Disabled
    {
        get => _disabled;
        set
        {
            _disabled = value;
            UpdateStyle();
        }
    }

    protected override IEnumerable<IComponent> SubComponents => [_label];

    private readonly Label _label;
    private readonly Label.TextContent _icon;
    private readonly Label.TextContent? _text;

    public override RenderableNode Node => _label.Node;

    public Button(string icon, string? text = null)
    {
        _icon = new(icon);
        if (text != null)
            _text = new(text);

        _label = new(_text == null ? [_icon] : [_icon, _text], true);

        UpdateStyle();
    }

    protected override void UpdateStyle()
    {
        if (_text != null)
        {
            _text.BackgroundColor = IsFocused && !Disabled ? Color.Slate.Dark : Color.Slate.Light;
            _text.Strikethrough = Disabled;
            _text.Bold = !Disabled;
        }

        _icon.BackgroundColor = Disabled
            ? Color.Slate.Light
            : IsFocused
                ? Accent.Regular
                : Color.Slate.Regular;
    }
}

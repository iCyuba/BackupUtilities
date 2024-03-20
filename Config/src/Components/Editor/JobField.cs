using BackupUtilities.Config.Components.Base;
using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;

namespace BackupUtilities.Config.Components.Editor;

public sealed class JobField : BaseButton
{
    public string Value
    {
        get => _value.Text;
        set => _value.Text = value;
    }

    protected override IEnumerable<IComponent> SubComponents =>
        [_label, _icon, _text, _separator, _value];

    private readonly Label _label;
    private readonly Label.TextContent _icon;
    private readonly Label.TextContent _text;
    private readonly Label.Content _separator = new() { Style = Label.Content.ContentStyle.None };
    private readonly Label.TextContent _value = new("") { Bold = true};

    public override RenderableNode Node => _label.Node;

    public JobField(string icon, string text)
    {
        _icon = new(icon);
        _text = new(text)
        {
            Style = Label.Content.ContentStyle.None,
            Bold = true,
            Color = Color.Slate.Dark
        };

        _label = new() { Children = [_icon, _text, _separator, _value] };

        UpdateStyle();
    }

    protected override void UpdateStyle()
    {
        _icon.BackgroundColor = IsFocused ? Color.Primary.Dark : Color.Slate.Light;
        _value.BackgroundColor = IsFocused ? Color.Slate.Regular : Color.Slate.Light;
    }
}

using BackupUtilities.Config.Components.Base;
using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Job;

/// <summary>
/// Stylized button for the "New job" button
/// </summary>
public sealed class NewButton : BaseButton
{
    protected override IEnumerable<IComponent> SubComponents => [_label, _icon, _text];

    private readonly FancyNode _container =
        new() { Color = Color.FromHex("#e2e8f0"), FlexShrink = 0 };

    private readonly Label _label = new() { Gap = true };
    private readonly Label.TextContent _icon = new("");
    private readonly Label.TextContent _text =
        new("New job")
        {
            Color = Color.Slate.Dark,
            Bold = true,
            Style = Label.Content.ContentStyle.None
        };

    public override RenderableNode Node => _container;

    public NewButton()
    {
        _label.Children = [_icon, _text];

        _container.SetBorder(Edge.Vertical, 1);
        _container.SetBorder(Edge.Horizontal, 2);
        _container.SetGap(Gutter.Row, 1);
        _container.SetChildren([_label.Node]);

        UpdateStyle();
    }

    protected override void UpdateStyle() =>
        _icon.BackgroundColor = IsFocused ? Color.Primary.Dark : Color.Slate.Light;
}

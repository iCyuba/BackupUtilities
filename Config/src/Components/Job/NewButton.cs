using BackupUtilities.Config.Components.Base;
using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Job;

/// <summary>
/// Stylized button for the "New job" button
/// </summary>
public sealed class NewButton : BaseComponent
{
    protected override IEnumerable<IComponent> SubComponents => [Button];

    private readonly FancyNode _container =
        new() { Color = Color.FromHex("#e2e8f0"), FlexShrink = 0 };

    public Button Button { get; } = new("", "New job");

    public override RenderableNode Node => _container;

    public NewButton()
    {
        _container.SetBorder(Edge.Vertical, 1);
        _container.SetBorder(Edge.Horizontal, 2);
        _container.SetGap(Gutter.Row, 1);
        _container.SetChildren([Button.Node]);

        UpdateStyle();
    }
}

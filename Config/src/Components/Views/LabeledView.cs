using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Views;

public abstract class LabeledView : BaseView
{
    private readonly FancyNode _labelContainer =
        new() { FlexDirection = FlexDirection.Row, JustifyContent = Justify.SpaceBetween, };

    protected readonly Label Label = new();
    private readonly Label _branding =
        new()
        {
            Children =
            [
                new Label.Content("") { BackgroundColor = Color.Pink.Light },
                new Label.Content("Backup Utilities") { Bold = true }
            ]
        };

    protected readonly FancyNode Content =
        new() { FlexDirection = FlexDirection.Column, FlexGrow = 1 };

    protected override Node ChildHost => Content;

    protected LabeledView()
    {
        Node.FlexDirection = FlexDirection.Column;
        Node.SetGap(Gutter.Row, 1);

        base.AddChild(Label, false, false);
        base.AddChild(_branding, false, false);

        _labelContainer.SetChildren([Label.Node, _branding.Node]);
        Node.SetChildren([_labelContainer, Content]);
    }
}

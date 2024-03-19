using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Components.Windows;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components;

public abstract class BaseModal : BaseView, IWindow
{
    public event Action? Closed;

    protected readonly FancyNode Content = new();

    private readonly FancyNode _overlay =
        new()
        {
            PositionType = PositionType.Absolute,
            JustifyContent = Justify.Center,
            AlignItems = Align.Center
        };

    public override RenderableNode Node => _overlay;

    public override bool CapturesInput => true;

    public BaseModal()
    {
        _overlay.SetPosition(Edge.All, 0);

        _overlay.Color = Color.FromHex("#64748b40");
        _overlay.SetChildren([Content]);

        Content.MinWidth = new(20);
        Content.MinHeight = new(5);
        Content.MaxWidth = new(Unit.Percent, 75);
        Content.MaxHeight = new(Unit.Percent, 75);

        Content.Color = Color.White;
        Content.SetBorder(Edge.All, 1);
        Content.SetPadding(Edge.Horizontal, 1);
    }

    protected void OnClose() => Closed?.Invoke();

    public override void HandleInput(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Escape)
            Closed?.Invoke();
    }
}

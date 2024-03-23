using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Components.Windows;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components;

/// <summary>
/// Base component for modals
/// </summary>
public abstract class BaseModal : TabView, IWindow
{
    public event Action? Closed;

    public Title Title { get; } = new();

    protected readonly FancyNode Content = new() { FlexGrow = 1 };
    private readonly FancyNode _modal = new() { FlexDirection = FlexDirection.Column };
    private readonly FancyNode _overlay =
        new()
        {
            PositionType = PositionType.Absolute,
            JustifyContent = Justify.Center,
            AlignItems = Align.Center
        };

    public override RenderableNode Node => _overlay;

    public override bool CapturesInput => true;

    protected BaseModal()
    {
        _overlay.SetPosition(Edge.All, 0);
        _overlay.SetBorder(Edge.All, 1);

        _overlay.Color = Color.FromHex("#64748b40");
        _overlay.SetChildren([_modal]);

        _modal.MinWidth = new(20);
        _modal.MinHeight = new(5);
        _modal.MaxWidth = new(Unit.Percent, 75);
        _modal.MaxHeight = new(Unit.Percent, 75);

        _modal.Color = Color.White;
        _modal.SetBorder(Edge.All, 1);
        _modal.SetPadding(Edge.Horizontal, 1);
        _modal.SetGap(Gutter.Row, 1);
        _modal.SetChildren([Title.Node, Content]);

        Title.Register(this);
    }

    protected void OnClose() => Closed?.Invoke();

    public override void HandleInput(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Escape)
            Closed?.Invoke();
        else
            base.HandleInput(key);
    }
}

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

    public string Icon
    {
        get => _title.Icon;
        set => _title.Icon = value;
    }

    public string Title
    {
        get => _title.Text;
        set => _title.Text = value;
    }

    private readonly Title _title = new();
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

        _overlay.BackgroundColor = Color.Overlay;
        _overlay.SetChildren([_modal]);

        _modal.MinWidth = new(20);
        _modal.MinHeight = new(5);
        _modal.MaxWidth = new(Unit.Percent, 75);
        _modal.MaxHeight = new(Unit.Percent, 75);

        _modal.BackgroundColor = Color.App.Primary;
        _modal.SetBorder(Edge.All, 1);
        _modal.SetPadding(Edge.Horizontal, 1);
        _modal.SetGap(Gutter.Row, 1);
        _modal.SetChildren([_title.Node, Content]);

        _title.Register(this);
    }

    protected void Close() => Closed?.Invoke();

    public override void HandleInput(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Escape)
            Closed?.Invoke();
        else
            base.HandleInput(key);
    }
}

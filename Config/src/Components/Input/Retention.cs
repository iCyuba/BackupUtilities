using BackupUtilities.Config.Components.Base;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;
using BackupUtilities.Shared;

namespace BackupUtilities.Config.Components.Generic;

/// <summary>
/// Input for setting the retention of a backup job.
/// </summary>
public sealed class Retention : BaseComponent, IInput<BackupJob.BackupRetention>
{
    public event Action? Updated;

    public BackupJob.BackupRetention Value
    {
        get => new() { Size = _size.Value, Count = _count.Value };
        set
        {
            _size.Value = value.Size;
            _count.Value = value.Count;
        }
    }

    protected override IEnumerable<IComponent> SubComponents => [_size, _count];

    private readonly FancyNode _sizeContainer =
        new() { JustifyContent = Justify.SpaceBetween, FlexGrow = 1 };
    private readonly TextNode _sizeLabel = new("Size") { Color = Color.Slate.Dark };
    private readonly NumberBox _size = new() { Min = 0 };

    private readonly FancyNode _countContainer =
        new() { JustifyContent = Justify.SpaceBetween, FlexGrow = 1 };
    private readonly TextNode _countLabel = new("Count") { Color = Color.Slate.Dark };
    private readonly NumberBox _count = new() { Min = 0 };

    private readonly FancyNode _container =
        new() { FlexDirection = FlexDirection.Column, FlexGrow = 1 };

    public override RenderableNode Node => _container;

    public Retention()
    {
        _sizeContainer.SetChildren([_sizeLabel, _size.Node]);
        _sizeContainer.SetGap(Gutter.Column, 1);

        _countContainer.SetChildren([_countLabel, _count.Node]);
        _countContainer.SetGap(Gutter.Column, 1);

        _container.SetChildren([_sizeContainer, _countContainer]);
        _container.SetGap(Gutter.Row, 1);

        _size.Updated += () => Updated?.Invoke();
        _count.Updated += () => Updated?.Invoke();
    }
}

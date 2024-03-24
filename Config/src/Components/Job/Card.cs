using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Components.Input;
using BackupUtilities.Config.Components.Modals;
using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;
using BackupUtilities.Shared;

namespace BackupUtilities.Config.Components.Job;

public sealed class Card : UpDownView
{
    public event Action? Updated;
    public event Action? Deleted;

    public BackupJob Job { get; }
    public int Index
    {
        set => _title.Text = $"{value + 1}";
    }

    public bool Valid => _sources.Valid && _targets.Valid && _timing.Valid;

    private readonly Field<IEnumerable<string>> _sources;
    private readonly Field<IEnumerable<string>> _targets;
    private readonly Field<string> _timing;

    private readonly FancyNode _container = new();
    public override RenderableNode Node => _container;

    private readonly Title _title = new() { Icon = "" };
    private readonly Button _delete = new("", "Delete") { Accent = Color.Red };

    public Card(BackupJob job)
    {
        Job = job;

        _sources = new(
            () => Job.Sources,
            s => s.Count().FormatNumber(),
            new PathList(),
            s => s.Any()
        );

        _targets = new(
            () => Job.Targets,
            t => t.Count().FormatNumber(),
            new PathList(),
            s => s.Any()
        );

        Field<BackupJob.BackupMethod> method =
            new(() => Job.Method, m => $"{m}"[..4], new Radio<BackupJob.BackupMethod>());

        _timing = new(() => Job.Timing, _ => "", new TextBox(), Cron.Validate);

        Field<BackupJob.BackupRetention> retention =
            new(
                () => Job.Retention,
                r => $"{r.Size.FormatNumber()}:{r.Count.FormatNumber()}",
                new Retention()
            );

        Field<IEnumerable<string>> ignore =
            new(() => Job.Ignore, i => i.Count().FormatNumber(), new List<string, TextBox>());

        Field<BackupJob.BackupOutput> output =
            new(
                () => Job.Output,
                o => o == BackupJob.BackupOutput.Folder ? "" : "",
                new Radio<BackupJob.BackupOutput>()
            );

        _container.FlexShrink = 0;
        _container.FlexDirection = FlexDirection.Column;
        _container.Color = Color.FromHex("#e2e8f0");

        _container.SetBorder(Edge.Horizontal, 2);
        _container.SetBorder(Edge.Vertical, 1);
        _container.SetGap(Gutter.Row, 1);

        _delete.Node.SetMarginAuto(Edge.Horizontal);

        _container.SetChildren(
            [
                _title.Node,
                _sources.Node,
                _targets.Node,
                method.Node,
                _timing.Node,
                retention.Node,
                ignore.Node,
                output.Node,
                _delete.Node
            ]
        );

        _title.Register(this);

        _sources.Register(this);
        _targets.Register(this);
        method.Register(this);
        _timing.Register(this);
        retention.Register(this);
        ignore.Register(this);
        output.Register(this);
        _delete.Register(this);

        _sources.Updated += Update;
        _targets.Updated += Update;
        method.Updated += Update;
        _timing.Updated += Update;
        retention.Updated += Update;
        ignore.Updated += Update;
        output.Updated += Update;
        _delete.Clicked += Delete;
    }

    private void Update() => Updated?.Invoke();

    private void Delete()
    {
        ConfirmModal modal =
            new("Are you sure you want to delete this backup job?")
            {
                Title = "Delete",
                Destructive = true
            };

        OpenModal(modal);

        modal.Confirmed += () => Deleted?.Invoke();
    }
}

using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Components.Input;
using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;
using BackupUtilities.Shared;

namespace BackupUtilities.Config.Components.Job;

public sealed class Card : UpDownView
{
    public BackupJob Job { get; }
    public int Index
    {
        set => _title.Text = $"{value + 1}";
    }

    private readonly FancyNode _container = new();
    public override RenderableNode Node => _container;

    private readonly Title _title = new() { Icon = "" };

    public Card(BackupJob job)
    {
        Job = job;

        Field<IEnumerable<string>> sources =
            new(() => Job.Sources, s => s.Count().FormatNumber(), new PathList());

        Field<IEnumerable<string>> targets =
            new(() => Job.Targets, t => t.Count().FormatNumber(), new PathList());

        Field<BackupJob.BackupMethod> method =
            new(() => Job.Method, m => $"{m}"[..4], new Radio<BackupJob.BackupMethod>());

        Field<string> timing = new(() => Job.Timing, _ => "", new TextBox());

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

        _container.SetChildren(
            [
                _title.Node,
                sources.Node,
                targets.Node,
                method.Node,
                timing.Node,
                retention.Node,
                ignore.Node,
                output.Node
            ]
        );

        _title.Register(this);

        sources.Register(this);
        targets.Register(this);
        method.Register(this);
        timing.Register(this);
        retention.Register(this);
        ignore.Register(this);
        output.Register(this);
    }
}

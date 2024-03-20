using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;
using BackupUtilities.Shared;

namespace BackupUtilities.Config.Components.Editor;

public sealed class JobCard : BaseView
{
    private readonly BackupJob _job;
    public int Index
    {
        init => _titleIndex.Text = $"{value + 1}";
    }

    private readonly FancyNode _container = new();
    public override RenderableNode Node => _container;

    private readonly Label _title = new();
    private readonly Label.TextContent _titleIcon =
        new("") { BackgroundColor = Color.Slate.Regular };
    private readonly Label.TextContent _titleIndex =
        new("0") { BackgroundColor = Color.Slate.Light };

    private readonly JobField _sources = new("", "Sources");
    private readonly JobField _targets = new("", "Targets");
    private readonly JobField _method = new("", "Method");
    private readonly JobField _timing = new("", "Timing") { Value = "" }; // I'm not gonna show any actual icon here.
    private readonly JobField _retention = new("", "Retention");
    private readonly JobField _ignore = new("", "Ignore");
    private readonly JobField _output = new("", "Output");

    public JobCard(BackupJob job)
    {
        _job = job;

        _title.Children = [_titleIcon, _titleIndex];
        _title.Node.SetMarginAuto(Edge.Horizontal);

        _container.FlexShrink = 0;
        _container.FlexDirection = FlexDirection.Column;
        _container.Color = Color.FromHex("#e2e8f0");

        _container.SetBorder(Edge.Horizontal, 2);
        _container.SetBorder(Edge.Vertical, 1);
        _container.SetGap(Gutter.Row, 1);

        _container.SetChildren(
            [
                _title.Node,
                _sources.Node,
                _targets.Node,
                _method.Node,
                _timing.Node,
                _retention.Node,
                _ignore.Node,
                _output.Node
            ]
        );

        _title.Register(this);
        _titleIcon.Register(this);
        _titleIndex.Register(this);

        _sources.Register(this);
        _targets.Register(this);
        _method.Register(this);
        _timing.Register(this);
        _retention.Register(this);
        _ignore.Register(this);
        _output.Register(this);

        UpdateStyle();
    }

    public override void HandleInput(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.UpArrow && !CapturesInput)
            FocusPrevious();
        else if (key.Key == ConsoleKey.DownArrow && !CapturesInput)
            FocusNext();
        else
            base.HandleInput(key);
    }

    protected override void UpdateStyle()
    {
        _sources.Value = $"{_job.Sources.Count}";
        _targets.Value = $"{_job.Targets.Count}";
        _method.Value = $"{_job.Method}"[..4];
        _retention.Value = $"{_job.Retention.Size}-{_job.Retention.Count}";
        _ignore.Value = $"{_job.Ignore.Count}";
        _output.Value = _job.Output switch
        {
            BackupJob.BackupOutput.Folder => "",
            _ => "",
        };
    }
}

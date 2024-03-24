using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Components.Job;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;
using BackupUtilities.Shared;

namespace BackupUtilities.Config.Components.Windows;

public sealed class EditorWindow : BaseWindow
{
    public List<BackupJob> Jobs
    {
        get => _jobList.Jobs.ToList();
        set => _jobList.Jobs = value;
    }

    private bool Valid => _jobList.Valid;

    private readonly JobList _jobList;
    private readonly Button _save = new("", "Save");
    private readonly Button _share = new("", "Share");
    private readonly Button _exit = new("", "Exit");
    private readonly FancyNode _buttonContainer = new() { JustifyContent = Justify.SpaceBetween };

    public EditorWindow(IEnumerable<BackupJob> jobs, App app)
        : base(app)
    {
        _jobList = new(jobs);

        Title.Icon = "";
        Title.Text = $"{Jobs.Count} Backup Job{(Jobs.Count == 1 ? "" : "s")}";

        Content.FlexDirection = FlexDirection.Column;
        Content.JustifyContent = Justify.SpaceBetween;

        _buttonContainer.SetMargin(Edge.All, 1);
        _buttonContainer.SetChildren([_save.Node, _share.Node, _exit.Node]);

        Content.SetChildren([_jobList.Node, _buttonContainer]);

        _exit.Clicked += OnClose;
        _jobList.Updated += UpdateStyle;

        _exit.Register(this);
        _jobList.Register(this);

        // Focus the job list by default
        FocusNext();

        UpdateStyle();
    }

    protected override void UpdateStyle()
    {
        base.UpdateStyle();

        _save.Color = Valid ? Color.Primary : Color.Slate;
        _share.Color = Valid ? Color.Primary : Color.Slate;

        // Enable / Disable the buttons
        if (Valid)
        {
            _save.Register(this);
            _share.Register(this);
        }
        else
        {
            _save.Unregister();
            _share.Unregister();
        }
    }
}

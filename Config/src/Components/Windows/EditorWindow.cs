using System.Text.Json;
using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Components.Job;
using BackupUtilities.Config.Components.Modals;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;
using BackupUtilities.Shared;
using NativeFileDialogSharp;

namespace BackupUtilities.Config.Components.Windows;

public sealed class EditorWindow : BaseWindow
{
    public List<BackupJob> Jobs
    {
        get => _jobList.Jobs.ToList();
        set => _jobList.Jobs = value;
    }

    public string SavePath { get; set; } = "";

    private bool Valid => _jobList.Valid;

    private readonly JobList _jobList;
    private readonly Button _save = new("", "Save");
    private readonly Button _saveAs = new("", "Save As");
    private readonly Button _share = new("", "Share");
    private readonly Button _exit = new("", "Exit");
    private readonly FancyNode _buttonContainer = new() { JustifyContent = Justify.SpaceBetween };

    public EditorWindow(IEnumerable<BackupJob> jobs, App app)
        : base(app)
    {
        _jobList = new(jobs);

        Title.Icon = "";
        // Title text is set in UpdateStyle

        Content.FlexDirection = FlexDirection.Column;
        Content.JustifyContent = Justify.SpaceBetween;

        _buttonContainer.SetChildren([_save.Node, _saveAs.Node, _share.Node, _exit.Node]);

        Content.SetGap(Gutter.All, 1);
        Content.SetChildren([_jobList.Node, _buttonContainer]);

        _jobList.Updated += UpdateStyle;
        _save.Clicked += Save;
        _saveAs.Clicked += SaveAs;
        _share.Clicked += Share;
        _exit.Clicked += OnClose;

        _exit.Register(this);
        _jobList.Register(this);

        // Focus the job list by default
        FocusNext();

        UpdateStyle();
    }

    protected override void UpdateStyle()
    {
        Title.Text = $"{Jobs.Count} Backup Job{(Jobs.Count == 1 ? "" : "s")}";

        _save.Color = Valid ? Color.Primary : Color.Slate;
        _saveAs.Color = Valid ? Color.Primary : Color.Slate;
        _share.Color = Valid ? Color.Primary : Color.Slate;

        // Enable / Disable the buttons
        if (Valid)
        {
            _save.Register(this);
            _saveAs.Register(this);
            _share.Register(this);
        }
        else
        {
            _save.Unregister();
            _saveAs.Unregister();
            _share.Unregister();
        }
    }

    private void Save()
    {
        if (SavePath == "")
        {
            SaveAs();
            return;
        }

        string json = JsonSerializer.Serialize(Jobs, Json.SerializerOptionsPretty);
        File.WriteAllText(SavePath, json);

        // Show confirmation
        OpenModal(new AlertModal("Backup jobs saved!") { Title = "Saved" });
    }

    private void SaveAs()
    {
        var path = Dialog.FileSave("json");
        if (path.IsOk)
        {
            SavePath = path.Path;
            Save();
        }
        else
            // Show an alert
            OpenModal(new AlertModal("No path selected.") { Title = "Invalid path", Icon = "" });
    }

    private void Share()
    {
        string id = SharingClient.Upload(Jobs).Result;

        // Create a modal with the ID
        OpenModal(
            new AlertModal($"Backup jobs shared!\nLink: {SharingClient.BASE}/{id}")
            {
                Title = "Shared"
            }
        );
    }
}

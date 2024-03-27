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

    private readonly CardList _jobList;
    private readonly Button _save = new("", "Save");
    private readonly Button _saveAs = new("", "Save As");
    private readonly Button _share = new("", "Share");
    private readonly Button _exit = new("", "Exit");
    private readonly FancyNode _buttonContainer = new() { JustifyContent = Justify.SpaceBetween };

    public EditorWindow(IEnumerable<BackupJob> jobs, App app)
        : base(app)
    {
        _jobList = new(jobs);

        Icon = "";
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
        _exit.Clicked += Exit;

        _exit.Register(this);
        _jobList.Register(this);

        // Focus the job list by default
        FocusNext();

        UpdateStyle();
    }

    public EditorWindow(App app)
        : this([], app) { }

    protected override void UpdateStyle()
    {
        Title = $"{Jobs.Count} Backup Job{(Jobs.Count == 1 ? "" : "s")}";

        _save.Disabled = !Valid;
        _saveAs.Disabled = !Valid;
        _share.Disabled = !Valid;

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

        try
        {
            string json = JsonSerializer.Serialize(Jobs, Json.SerializerOptionsPretty);
            File.WriteAllText(SavePath, json);
        }
        catch
        {
            OpenModal(new AlertModal("Could not save file!") { Icon = "", Title = "Error" });
            return;
        }

        // Show confirmation
        OpenModal(new AlertModal("Backup jobs saved!") { Title = "Saved" });
    }

    private void SaveAs()
    {
        try
        {
            // Try opening the native file picker
            var path = Dialog.FileSave("json");
            if (path.IsOk)
            {
                SavePath = path.Path;
                Save();
            }
            else
                ShowAlert();
        }
        catch
        {
            // Fall back to a modal with a textbox
            InputModal<string> modal = new(new TextBox()) { Icon = "", Title = "File path" };
            OpenModal(modal);

            modal.Closed += () =>
            {
                if (modal.Value.EndsWith(".json"))
                    SavePath = modal.Value;
                else
                    ShowAlert();
            };
        }

        // Show an alert
        void ShowAlert()
        {
            OpenModal(new AlertModal("No path selected.") { Title = "Invalid path", Icon = "" });
        }
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

    private void Exit()
    {
        ConfirmModal modal =
            new("Are you sure you want to exit? Any unsaved changes will be lost. ")
            {
                Title = "Exit",
                Destructive = true
            };

        OpenModal(modal);
        modal.Confirmed += Close;
    }
}

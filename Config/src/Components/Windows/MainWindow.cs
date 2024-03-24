using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Components.Modals;
using BackupUtilities.Config.Yoga;
using BackupUtilities.Shared;
using NativeFileDialogSharp;

namespace BackupUtilities.Config.Components.Windows;

public class MainWindow : BaseWindow
{
    private readonly Button _new = new("", "New config");
    private readonly Button _file = new("", "Open config");
    private readonly Button _shared = new("", "Open shared config");
    private readonly Button _quit = new("", "Quit");

    public MainWindow(App app)
        : base(app)
    {
        Title.Icon = "";
        Title.Text = "Config Editor";

        Content.FlexDirection = FlexDirection.Row;
        Content.JustifyContent = Justify.Center;
        Content.AlignItems = Align.Center;
        Content.SetGap(Gutter.All, 1);

        Content.SetChildren([_new.Node, _file.Node, _shared.Node, _quit.Node]);

        _new.Clicked += () => app.SetWindow(new EditorWindow([new()], App));
        _file.Clicked += File;
        _shared.Clicked += Shared;
        _quit.Clicked += OnClose;

        _new.Register(this);
        _file.Register(this);
        _shared.Register(this);
        _quit.Register(this);
    }

    private void File()
    {
        var path = Dialog.FileOpen("json");

        if (!path.IsOk)
        {
            // Show an alert if the user cancels the dialog
            OpenModal(new AlertModal("No file selected.") { Title = "Error", Icon = "" });
            return;
        }

        var jobs = BackupJob.LoadFromConfig(path.Path, false);
        App.SetWindow(new EditorWindow(jobs, App) { SavePath = path.Path });
    }

    private void Shared()
    {
        // Ask for the url or id
        InputModal<string> modal = new(new TextBox()) { Title = "Enter id or url", Icon = "" };
        OpenModal(modal);

        modal.Closed += () =>
        {
            string id = modal.Value;

            try
            {
                var jobs = SharingClient.Get(id).Result;
                App.SetWindow(new EditorWindow(jobs, App));
            }
            catch
            {
                OpenModal(new AlertModal("Invalid id or url.") { Title = "Error", Icon = "" });
            }
        };
    }
}

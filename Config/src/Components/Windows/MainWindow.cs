using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Components.Input;
using BackupUtilities.Config.Components.Modals;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Yoga;
using BackupUtilities.Shared;
using NativeFileDialogSharp;

namespace BackupUtilities.Config.Components.Windows;

public class MainWindow : BaseWindow
{
    private readonly FancyNode _buttons =
        new()
        {
            JustifyContent = Justify.SpaceEvenly,
            AlignItems = Align.Center,
            FlexWrap = Wrap.Wrap,
            MaxWidth = new(Unit.Percent, 75),
            MaxHeight = new(Unit.Percent, 50),
        };

    private readonly Button _new = new("", "New config");
    private readonly Button _file = new("", "Open config");
    private readonly Button _shared = new("", "Open shared config");
    private readonly Button _quit = new("", "Quit");

    public MainWindow(App app)
        : base(app)
    {
        Icon = "";
        Title = "Config Editor";

        _buttons.SetChildren([_new.Node, _file.Node, _shared.Node, _quit.Node]);
        _buttons.SetGap(Gutter.Column, 6);
        _buttons.SetGap(Gutter.Row, 2);

        Content.JustifyContent = Justify.Center;
        Content.AlignItems = Align.Center;
        Content.SetGap(Gutter.All, 5);
        Content.SetChildren([_buttons]);

        _new.Clicked += () => app.SetWindow(new EditorWindow(App));
        _file.Clicked += File;
        _shared.Clicked += Shared;
        _quit.Clicked += Close;

        _new.Register(this);
        _file.Register(this);
        _shared.Register(this);
        _quit.Register(this);
    }

    private void File()
    {
        try
        {
            if (Environment.GetCommandLineArgs().Contains("--no-dialog"))
                throw new();

            // Try opening the native file picker
            var path = Dialog.FileOpen("json");

            if (!path.IsOk)
            {
                ShowAlert();
                return;
            }

            Open(path.Path);
        }
        catch
        {
            // Fallback to a textbox modal
            InputModal<string> modal = new(new TextBox()) { Icon = "", Title = "File path" };
            OpenModal(modal);

            modal.Closed += () =>
            {
                if (modal.Value.EndsWith(".json"))
                    Open(modal.Value);
                else
                    ShowAlert();
            };
        }
        return;

        // Show an alert if the user cancels the dialog / inputs an invalid path
        void ShowAlert() =>
            OpenModal(new AlertModal("No file selected.") { Title = "Error", Icon = "" });

        void Open(string path)
        {
            try
            {
                var jobs = BackupJob.LoadFromConfig(path, false);
                App.SetWindow(new EditorWindow(jobs, App) { SavePath = path });
            }
            catch
            {
                OpenModal(new AlertModal("Could not open file!") { Icon = "", Title = "Error" });
            }
        }
    }

    private void Shared()
    {
        // Ask for the url or id
        InputModal<string> modal =
            new(new TextBox())
            {
                Title = "Enter id or url",
                Icon = "",
                Value = SharingClient.BASE + "/",
            };
        OpenModal(modal);

        modal.Closed += () =>
        {
            string uri = modal.Value;

            try
            {
                var jobs = SharingClient.Get(SharingClient.ParseIdFromUri(uri) ?? "").Result;
                App.SetWindow(new EditorWindow(jobs, App));
            }
            catch
            {
                OpenModal(new AlertModal("Invalid url.") { Title = "Error", Icon = "" });
            }
        };
    }
}

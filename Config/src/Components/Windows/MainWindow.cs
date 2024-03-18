using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Windows;

public class MainWindow : BaseWindow
{
    private readonly Button _new = new("", "New config");
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

        Content.SetChildren([_new.Node, _quit.Node]);

        _new.Clicked += () => throw new NotImplementedException();
        _quit.Clicked += OnClose;

        _new.Register(this);
        _quit.Register(this);
    }
}

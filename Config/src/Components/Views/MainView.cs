using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Views;

public class MainView : LabeledView
{
    private readonly Button _new = new("", "New config");
    private readonly Button _quit = new("", "Quit");

    public MainView()
    {
        Label.Children =
        [
            new Label.Content("") { BackgroundColor = Color.Pink.Light },
            new Label.Content("Config Editor") { Bold = true, },
        ];

        Content.FlexDirection = FlexDirection.Row;
        Content.JustifyContent = Justify.Center;
        Content.AlignItems = Align.Center;
        Content.SetGap(Gutter.All, 1);

        base.AddChild(_new);
        base.AddChild(_quit);

        _new.Clicked += () => throw new NotImplementedException();
        _quit.Clicked += Close;
    }
}

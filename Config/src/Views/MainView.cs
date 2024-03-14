using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Views;

public class MainView : BaseView
{
    private readonly TextNode _titleNode =
        new("Backup Utilities") { Color = Color.FromHex("#FFD700") };

    public MainView(App app)
        : base(app)
    {
        Node.AlignItems = Align.Center;
        Node.JustifyContent = Justify.Center;

        Node.InsertChild(_titleNode, 0);
    }

    public override void HandleInput(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Q)
            Close();
    }
}

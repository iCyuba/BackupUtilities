using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Views;

public class MainView : BaseView
{
    private readonly FancyNode _titleContainer = new() { Color = Color.FromHex("#f472b6") };

    private readonly TextNode _titleNode =
        new("  Backup Utilities") { Color = Color.FromHex("#831843"), Bold = true };

    public MainView(App app)
        : base(app)
    {
        Node.AlignItems = Align.Center;
        Node.JustifyContent = Justify.Center;

        _titleContainer.SetBorder(Edge.Horizontal, 1);
        _titleContainer.SetPadding(Edge.Horizontal, 1);
        _titleContainer.InsertChild(_titleNode, 0);

        Node.InsertChild(_titleContainer, 0);
    }

    public override void HandleInput(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Q)
            Close();
    }
}

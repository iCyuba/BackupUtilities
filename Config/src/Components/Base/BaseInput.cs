using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Base;

public abstract class BaseInput : BaseInteractive
{
    private char Cursor => IsFocused ? '▏' : ' ';
    protected string Text
    {
        get => _text.Text[..^1];
        set => _text.Text = value + Cursor;
    }

    private readonly FancyNode _container =
        new() { Color = Color.FromHex("#f1f5f9"), FlexGrow = 1 };

    private readonly TextNode _text = new(" ") { Color = Color.Slate.Dark, Trim = false };

    public override RenderableNode Node => _container;

    protected BaseInput()
    {
        Focused += UpdateStyle;
        Blurred += UpdateStyle;

        _container.SetBorder(Edge.Horizontal, 1);
        _container.SetPadding(Edge.Horizontal, 1);
        _container.SetChildren([_text]);
    }

#pragma warning disable CA2245 // Do not assign a property to itself
    protected override void UpdateStyle() => Text = Text;
#pragma warning restore CA2245 // Do not assign a property to itself
}

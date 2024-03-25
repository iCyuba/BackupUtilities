using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Base;

/// <summary>
/// Base class for text based input components.
/// </summary>
/// <typeparam name="T">Type of the input value.</typeparam>
public abstract class BaseTextInput<T> : BaseInteractive, IInput<T>
{
    public abstract event Action? Updated;
    public abstract T Value { get; set; }

    private char Cursor => IsFocused ? '▏' : ' ';
    protected string Text
    {
        get => _text.Text[..^1];
        set => _text.Text = value + Cursor;
    }

    private readonly FancyNode _container =
        new() { BackgroundColor = Color.Element.Tertiary, FlexGrow = 1 };

    private readonly TextNode _text = new(" ") { Color = Color.Foreground.Secondary, Trim = false };

    public override RenderableNode Node => _container;

    protected BaseTextInput()
    {
        Focused += UpdateStyle;
        Blurred += UpdateStyle;

        _container.SetBorder(Edge.Horizontal, 1);
        _container.SetPadding(Edge.Horizontal, 1);
        _container.SetChildren([_text]);
    }

    protected override void UpdateStyle()
    {
        _text.Text = Text + Cursor;
        _text.Color = IsFocused ? Color.Foreground.Primary : Color.Foreground.Secondary;
    }
}

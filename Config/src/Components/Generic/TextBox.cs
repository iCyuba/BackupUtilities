using BackupUtilities.Config.Components.Base;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Generic;

public sealed class TextBox : BaseInteractive
{
    public event Action<string>? Updated;

    public string Value
    {
        get => _text.Text[..^1];
        set => _text.Text = value + "▏";
    }

    public bool Multiline { get; set; } = true;

    private readonly FancyNode _container =
        new() { Color = Color.FromHex("#f1f5f9"), FlexGrow = 1 };

    private readonly TextNode _text = new("▏") { Color = Color.Slate.Dark, Trim = false };

    public override RenderableNode Node => _container;

    public TextBox()
    {
        _container.SetBorder(Edge.Horizontal, 1);
        _container.SetPadding(Edge.Horizontal, 1);
        _container.SetChildren([_text]);
    }

    public override void HandleInput(ConsoleKeyInfo key)
    {
        if (!char.IsControl(key.KeyChar))
            Value += key.KeyChar;
        else if (key.Key == ConsoleKey.Backspace && Value.Length > 0)
            Value = Value[..^1];
        else if (key.Key == ConsoleKey.Enter && Multiline)
            Value += '\n';
        else
            return;

        Updated?.Invoke(Value);
    }
}

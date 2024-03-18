using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;

namespace BackupUtilities.Config.Components;

public class Button : BaseInteractive
{
    public enum ButtonStyle
    {
        Alternative,
        Regular,
    }

    public event Action? Clicked;

    private Color.Group _color = Util.Color.Primary;
    public Color.Group Color
    {
        get => _color;
        set
        {
            _color = value;
            UpdateStyle();
        }
    }

    public ButtonStyle Style => (ButtonStyle)_text.Style;

    protected override IEnumerable<IComponent> SubComponents => [_label, _icon, _text];

    private readonly Label _label;
    private readonly Label.TextContent _icon;
    private readonly Label.TextContent _text;

    public override RenderableNode Node => _label.Node;

    public Button(string icon, string text, ButtonStyle style = ButtonStyle.Regular)
    {
        _icon = new(icon);
        _text = new(text) { Style = (Label.Content.ContentStyle)style };
        _label = new() { Children = [_icon, _text], Gap = true };

        Focus += UpdateStyle;
        Blur += UpdateStyle;
        UpdateStyle();

        _label.Children = [_icon, _text];
    }

    public override void HandleInput(ConsoleKeyInfo key)
    {
        if (key.Key is ConsoleKey.Enter or ConsoleKey.Spacebar)
            Clicked?.Invoke();
    }

    private void UpdateStyle()
    {
        _text.Bold = IsFocused || Style == ButtonStyle.Alternative;
        _text.Color = Style == ButtonStyle.Alternative ? Util.Color.Slate.Dark : Util.Color.White;
        _text.BackgroundColor =
            Style == ButtonStyle.Alternative ? null : (IsFocused ? Color.Dark : Color.Regular);

        _icon.BackgroundColor = IsFocused
            ? (Style == ButtonStyle.Alternative ? Color.Dark : Color.Regular)
            : (Style == ButtonStyle.Alternative ? Util.Color.Slate.Light : Color.Light);
    }
}

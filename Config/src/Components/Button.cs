using BackupUtilities.Config.Util;

namespace BackupUtilities.Config.Components;

public class Button : BaseComponentWrapper, IInteractive
{
    public event Action? Clicked;

    private bool _isFocused;

    public bool IsFocused
    {
        get => _isFocused;
        set
        {
            _isFocused = value;
            UpdateStyle();
        }
    }

    private readonly Label.Content _icon;
    private readonly Label.Content _text;

    private Label Label => (Label)Component;

    public Button(string icon, string text)
        : base(new Label())
    {
        _icon = new(icon);
        _text = new(text);

        UpdateStyle();

        Label.Children = [_icon, _text];
    }

    public void HandleInput(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
            Clicked?.Invoke();
    }

    private void UpdateStyle()
    {
        if (_isFocused)
        {
            _icon.BackgroundColor = Color.Pink.Regular;
            _text.BackgroundColor = Color.Pink.Dark;
            _text.Bold = true;
        }
        else
        {
            _icon.BackgroundColor = Color.Pink.Light;
            _text.BackgroundColor = Color.Pink.Regular;
            _text.Bold = false;
        }
    }
}

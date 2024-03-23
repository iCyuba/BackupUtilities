using BackupUtilities.Config.Components.Base;

namespace BackupUtilities.Config.Components.Generic;

/// <summary>
/// Text input box.
/// </summary>
public sealed class TextBox : BaseTextInput<string>
{
    public override event Action? Updated;

    public override string Value
    {
        get => Text;
        set => Text = value;
    }

    public bool Multiline { get; set; } = true;

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

        Updated?.Invoke();
    }
}

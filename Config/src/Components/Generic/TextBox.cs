using BackupUtilities.Config.Components.Base;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Generic;

public sealed class TextBox : BaseInput
{
    public event Action<string>? Updated;

    public string Value
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

        Updated?.Invoke(Value);
    }
}

using BackupUtilities.Config.Components.Base;

namespace BackupUtilities.Config.Components.Generic;

public sealed class NumberBox : BaseInput
{
    public event Action<int>? Updated;

    public int Value
    {
        get
        {
            string text = Text;

            return text.Length == 0 ? 0 : int.Parse(text);
        }
        set => Text = $"{value}";
    }

    public int Min { get; set; } = int.MinValue;
    public int Max { get; set; } = int.MaxValue;

    public NumberBox() => Text = "0";

    public override void HandleInput(ConsoleKeyInfo key)
    {
        long value = Value;

        if (char.IsDigit(key.KeyChar))
        {
            long val = value * 10 + (long.Parse($"{key.KeyChar}") * (value < 0 ? -1 : 1));

            if (val <= Max && val >= Min)
                Value = (int)val;
        }
        else if (key.Key == ConsoleKey.Backspace && Text.Length > 0)
            Value /= 10;
        else if (key.Key == ConsoleKey.UpArrow && value < Max)
            Value++;
        else if (key.Key == ConsoleKey.DownArrow && value > Min)
            Value--;
        else if (key.KeyChar == '-' && value * -1 >= Min && value * -1 <= Max)
            Value *= -1;
        else
            return;

        Updated?.Invoke(Value);
    }
}

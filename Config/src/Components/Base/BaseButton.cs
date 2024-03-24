namespace BackupUtilities.Config.Components.Base;

/// <summary>
/// Base class for buttons.
/// </summary>
public abstract class BaseButton : BaseInteractive
{
    public event Action? Clicked;

    protected BaseButton()
    {
        Focused += UpdateStyle;
        Blurred += UpdateStyle;
    }

    public override void HandleInput(ConsoleKeyInfo key)
    {
        if (key.Key is ConsoleKey.Enter or ConsoleKey.Spacebar)
            Clicked?.Invoke();
    }
}

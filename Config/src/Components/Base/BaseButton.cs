namespace BackupUtilities.Config.Components.Base;

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

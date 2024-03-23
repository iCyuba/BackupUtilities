namespace BackupUtilities.Config.Components.Views;

/// <summary>
/// A view that can be navigated using the up and down arrow keys.
/// </summary>
public abstract class UpDownView : BaseView
{
    public override void HandleInput(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.UpArrow && !InputCapturedInside)
            FocusPrevious();
        else if (key.Key == ConsoleKey.DownArrow && !InputCapturedInside)
            FocusNext();
        else
            base.HandleInput(key);
    }
}

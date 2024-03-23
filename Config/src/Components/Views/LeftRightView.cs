namespace BackupUtilities.Config.Components.Views;

/// <summary>
/// A view that can be navigated using the left and right arrow keys.
/// </summary>
public abstract class LeftRightView : BaseView
{
    public override void HandleInput(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.LeftArrow && !InputCapturedInside)
            FocusPrevious();
        else if (key.Key == ConsoleKey.RightArrow && !InputCapturedInside)
            FocusNext();
        else
            base.HandleInput(key);
    }
}

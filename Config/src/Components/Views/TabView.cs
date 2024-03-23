namespace BackupUtilities.Config.Components.Views;

/// <summary>
/// A view that can be navigated using the tab key.
/// </summary>
public abstract class TabView : BaseView
{
    public override void HandleInput(ConsoleKeyInfo key)
    {
        if (key.Key != ConsoleKey.Tab || InputCapturedInside)
        {
            base.HandleInput(key);
            return;
        }

        if (key.Modifiers.HasFlag(ConsoleModifiers.Shift))
            FocusPrevious();
        else
            FocusNext();
    }
}

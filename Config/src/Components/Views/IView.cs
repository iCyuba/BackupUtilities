namespace BackupUtilities.Config.Components.Views;

/// <summary>
/// A view is a component that contains other interactive components that can be focused.<br/>
/// <br/>
/// The view is used by components such as the window or modal to show content.
/// </summary>
public interface IView : IInteractive
{
    event Action<IInteractive?, IInteractive?>? FocusChange;

    LinkedList<IInteractive> Interactive { get; }
    IInteractive? Active { get; }

    bool InputCapturedInside { get; }

    void FocusNext();
    void FocusPrevious();
    void FocusNearest();
    void Focus(IInteractive? interactive);
}

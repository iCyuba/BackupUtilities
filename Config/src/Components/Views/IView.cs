namespace BackupUtilities.Config.Components.Views;

public interface IView : IInteractive
{
    event Action<IInteractive?, IInteractive?>? FocusChange;

    LinkedList<IInteractive> Interactive { get; }
    IInteractive? Active { get; }

    void FocusNext();
    void FocusPrevious();
}

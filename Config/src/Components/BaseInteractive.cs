using BackupUtilities.Config.Components.Views;

namespace BackupUtilities.Config.Components;

public abstract class BaseInteractive : BaseComponent, IInteractive
{
    protected event Action? Focus;
    protected event Action? Blur;

    private LinkedListNode<IInteractive>? _focusNode;

    public virtual bool IsFocused => View is { IsFocused: true } && View?.Active == this;

    public virtual void HandleInput(ConsoleKeyInfo key) { }

    public override void Register(IView view)
    {
        base.Register(view);

        _focusNode = view.Interactive.AddLast(this);
        view.FocusChange += ViewFocusChange;

        if (view.Active == null)
            view.FocusNext();
    }

    public override void Unregister()
    {
        if (View == null)
            return;

        View.Interactive.Remove(_focusNode!);
        View.FocusChange -= ViewFocusChange;

        if (View.Active == this)
            View.FocusPrevious();

        base.Unregister();
    }

    private void ViewFocusChange(IInteractive? previous, IInteractive? next)
    {
        if (previous == this)
            Blur?.Invoke();
        else if (next == this)
            Focus?.Invoke();
    }
}

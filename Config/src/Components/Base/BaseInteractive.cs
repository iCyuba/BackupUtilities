using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Util;

namespace BackupUtilities.Config.Components.Base;

/// <summary>
/// Base class for interactive components.
/// </summary>
public abstract class BaseInteractive : BaseComponent, IInteractive
{
    protected event Action? Focused;
    protected event Action? Blurred;

    private LinkedListNode<IInteractive>? _focusNode;

    public virtual bool IsFocused => View is { IsFocused: true } && View?.Active == this;
    public virtual bool CapturesInput => false;

    public virtual void HandleInput(ConsoleKeyInfo key) { }

    public virtual void HandleMouse(Mouse mouse) { }

    public override void Register(IView view)
    {
        base.Register(view);

        view.FocusChange += ViewFocusChange;

        if (view.Interactive.Contains(this))
            return;

        _focusNode = view.Interactive.AddLast(this);

        if (view.Active == null)
            view.FocusNext();

        if (view.Active == this)
            Focused?.Invoke();
    }

    public override void Unregister()
    {
        if (View == null)
            return;

        View.Interactive.Remove(_focusNode!);
        View.FocusChange -= ViewFocusChange;

        if (View.Active == this)
            View.FocusPrevious();

        Blurred?.Invoke();

        base.Unregister();
    }

    private void ViewFocusChange(IInteractive? previous, IInteractive? next)
    {
        if (previous == this)
            Blurred?.Invoke();
        else if (next == this)
            Focused?.Invoke();
    }
}

using BackupUtilities.Config.Components.Base;
using BackupUtilities.Config.Util;

namespace BackupUtilities.Config.Components.Views;

/// <summary>
/// Base class for views.
/// </summary>
public abstract class BaseView : BaseInteractive, IView
{
    public event Action<IInteractive?, IInteractive?>? FocusChange;

    protected sealed override IEnumerable<IComponent> SubComponents => [];

    public LinkedList<IInteractive> Interactive { get; } = [];
    private LinkedListNode<IInteractive>? _active;

    public IInteractive? Active => _active?.Value;

    public bool InputCapturedInside =>
        (Active?.CapturesInput ?? false) || Active is IView { InputCapturedInside: true };

    protected BaseView()
    {
        Focused += OnFocusChange;
        Blurred += OnFocusChange;
    }

    public void FocusNext()
    {
        var old = Active;

        _active = _active?.Next ?? Interactive.First;
        OnFocusChange(old);
    }

    public void FocusPrevious()
    {
        var old = Active;

        _active = _active?.Previous ?? Interactive.Last;
        OnFocusChange(old);
    }

    public void FocusNearest()
    {
        if (_active?.Next != null || _active?.Previous == null)
            FocusNext();
        else
            FocusPrevious();
    }

    public void Focus(IInteractive? interactive)
    {
        if (interactive == null)
            return;

        var old = Active;

        _active = Interactive.Find(interactive);
        OnFocusChange(old);
    }

    public override void HandleInput(ConsoleKeyInfo key) => Active?.HandleInput(key);

    public override void HandleMouse(Mouse mouse)
    {
        if (InputCapturedInside)
        {
            Active!.HandleMouse(mouse);
            return;
        }

        foreach (var interactive in Interactive)
        {
            float top = interactive.Node.ComputedTotalTop;
            float left = interactive.Node.ComputedTotalLeft;
            float width = interactive.Node.ComputedWidth;
            float height = interactive.Node.ComputedHeight;

            bool contained =
                mouse.Position.X > left
                && mouse.Position.X <= left + width
                && mouse.Position.Y > top
                && mouse.Position.Y <= top + height;

            if (!contained)
                continue;

            interactive.HandleMouse(mouse);
            Focus(interactive);

            break;
        }
    }

    protected virtual void OnFocusChange(IInteractive? old, IInteractive? current) =>
        FocusChange?.Invoke(old, current);

    protected void OnFocusChange(IInteractive? old) => OnFocusChange(old, Active);

    protected void OnFocusChange() => OnFocusChange(null);
}

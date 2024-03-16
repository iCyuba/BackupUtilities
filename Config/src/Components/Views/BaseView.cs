using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Views;

public abstract class BaseView : BaseComponent, IView
{
    public event Action? Closed;

    public bool IsFocused { get; set; }

    private LinkedList<IInteractive> _interactive = [];
    private LinkedListNode<IInteractive>? _focus;

    public IComponent? Focus => _focus?.Value;

    public App? App { get; set; }

    protected BaseView()
    {
        Node.FlexDirection = FlexDirection.Column;
        Node.FlexGrow = 1;
        Node.SetPadding(Edge.All, 1);

        Updated += UpdateInteractive;
    }

    public void HandleInput(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.Tab:
                if (key.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    FocusPrevious();
                else
                    FocusNext();
                break;

            default:
                _focus?.Value.HandleInput(key);
                break;
        }
    }

    protected void FocusNext()
    {
        var next = _focus?.Next ?? _interactive.First;

        if (next == null)
            return;

        if (_focus != null)
            _focus.Value.IsFocused = false;

        _focus = next;
        next.Value.IsFocused = true;
    }

    protected void FocusPrevious()
    {
        var previous = _focus?.Previous ?? _interactive.Last;

        if (previous == null)
            return;

        if (_focus != null)
            _focus.Value.IsFocused = false;

        _focus = previous;
        previous.Value.IsFocused = true;
    }

    protected void Close() => Closed?.Invoke();

    private void UpdateInteractive()
    {
        LinkedList<IInteractive> newInteractive = [];
        AddInteractive(this);

        // Find the focused interactive in the new list, or default to the first
        var oldFocus = _focus;
        _focus = FindFocused(_focus);
        _interactive = newInteractive;

        if (_focus?.Value != oldFocus?.Value)
        {
            if (oldFocus != null)
                oldFocus.Value.IsFocused = false;

            if (_focus != null)
                _focus.Value.IsFocused = true;
        }

        return;

        void AddInteractive(IComponent component)
        {
            if (component is IInteractive interactive && component != this)
                newInteractive.AddLast(interactive);

            foreach (var child in component.Children)
                AddInteractive(child);
        }

        LinkedListNode<IInteractive>? FindFocused(LinkedListNode<IInteractive>? start = null)
        {
            if (start == null)
                return newInteractive.First;

            return newInteractive.Find(start.Value) ?? FindFocused(start.Next);
        }
    }
}

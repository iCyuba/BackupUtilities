using BackupUtilities.Config.Components.Base;

namespace BackupUtilities.Config.Components.Views;

public abstract class BaseView : BaseInteractive, IView
{
    public event Action<IInteractive?, IInteractive?>? FocusChange;

    protected sealed override IEnumerable<IComponent> SubComponents => [];

    public LinkedList<IInteractive> Interactive { get; } = [];
    private LinkedListNode<IInteractive>? _active;

    public IInteractive? Active => _active?.Value;

    protected BaseView()
    {
        Focus += OnFocusChange;
        Blur += OnFocusChange;
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

    public override void HandleInput(ConsoleKeyInfo key) => Active?.HandleInput(key);

    protected virtual void OnFocusChange(IInteractive? old, IInteractive? current) =>
        FocusChange?.Invoke(old, current);

    protected void OnFocusChange(IInteractive? old) => OnFocusChange(old, Active);

    protected void OnFocusChange() => OnFocusChange(null);
}

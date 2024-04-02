using BackupUtilities.Config.Components.Input;

namespace BackupUtilities.Config.Components.Modals;

/// <summary>
/// A modal that contains an input.
/// </summary>
/// <typeparam name="T">Value type of the input.</typeparam>
public class InputModal<T> : BaseModal, IInput<T>
{
    public event Action? Updated;

    public T Value
    {
        get => Input.Value;
        set => Input.Value = value;
    }

    protected readonly IInput<T> Input;

    private readonly bool _closeOnEnter;

    public InputModal(IInput<T> input, bool closeOnEnter = true)
    {
        Input = input;
        _closeOnEnter = closeOnEnter;

        input.Register(this);
        Content.SetChildren([input.Node]);

        input.Updated += () => Updated?.Invoke();
    }

    public override void HandleInput(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Enter && _closeOnEnter)
            Close();
        else
            base.HandleInput(key);
    }
}

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

    public InputModal(IInput<T> input)
    {
        Input = input;

        input.Register(this);
        Content.SetChildren([input.Node]);

        input.Updated += () => Updated?.Invoke();
    }
}

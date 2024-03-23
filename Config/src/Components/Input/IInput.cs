namespace BackupUtilities.Config.Components;

public interface IInput<T> : IComponent
{
    /// <summary>
    /// Value of the input has changed.
    /// </summary>
    event Action? Updated;

    /// <summary>
    /// Any value that the input represents.
    /// </summary>
    T Value { get; set; }
}

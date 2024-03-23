namespace BackupUtilities.Config.Util;

/// <summary>
/// A factory that creates objects of type T.
/// </summary>
/// <typeparam name="T">Type to create.</typeparam>
public interface IFactory<out T>
{
    /// <summary>
    /// Creates an object of type T.
    /// </summary>
    /// <returns></returns>
    T Create();
}

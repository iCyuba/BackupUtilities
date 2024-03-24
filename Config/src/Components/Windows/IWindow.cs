using BackupUtilities.Config.Components.Views;

namespace BackupUtilities.Config.Components.Windows;

/// <summary>
/// A window that can be displayed on the screen.
/// </summary>
public interface IWindow : IView
{
    event Action? Closed;
}

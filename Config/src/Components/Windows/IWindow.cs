using BackupUtilities.Config.Components.Views;

namespace BackupUtilities.Config.Components.Windows;

public interface IWindow : IView
{
    event Action? Closed;
}

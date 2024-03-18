using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;

namespace BackupUtilities.Config.Components;

public interface IComponent
{
    RenderableNode Node { get; }

    void Register(IView view);
    void Unregister();
}

using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;

namespace BackupUtilities.Config.Components;

/// <summary>
/// A component that can be rendered to the screen.
/// </summary>
public interface IComponent
{
    /// <summary>
    /// The node that this component is associated with.
    /// </summary>
    RenderableNode Node { get; }

    void Register(IView view);
    void Unregister();
}

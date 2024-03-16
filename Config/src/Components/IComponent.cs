using BackupUtilities.Config.Nodes;

namespace BackupUtilities.Config.Components;

public interface IComponent
{
    event Action? Updated;
    IEnumerable<IComponent> Children { get; set; }

    IComponent? Parent { get; set; }
    RenderableNode Node { get; }

    void AddChild(IComponent child, bool insert = true, bool update = true);
    void RemoveChild(IComponent child, bool update = true);
    void SetChildren(IEnumerable<IComponent> children, bool insert = true, bool update = true);
}

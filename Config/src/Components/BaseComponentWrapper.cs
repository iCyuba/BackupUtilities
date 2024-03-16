using BackupUtilities.Config.Nodes;

namespace BackupUtilities.Config.Components;

public abstract class BaseComponentWrapper : IComponent
{
    public event Action? Updated;

    protected readonly IComponent Component;

    public IEnumerable<IComponent> Children
    {
        get => Component.Children;
        set => Component.Children = value;
    }

    public IComponent? Parent
    {
        get => Component.Parent;
        set => Component.Parent = value;
    }

    public RenderableNode Node => Component.Node;

    protected BaseComponentWrapper(IComponent component)
    {
        Component = component;
        Component.Updated += () => Updated?.Invoke();
    }

    public void AddChild(IComponent child, bool insert = true, bool update = true) =>
        Component.AddChild(child, insert, update);

    public void RemoveChild(IComponent child, bool update = true) =>
        Component.RemoveChild(child, update);

    public void SetChildren(
        IEnumerable<IComponent> children,
        bool insert = true,
        bool update = true
    ) => Component.SetChildren(children, insert, update);
}

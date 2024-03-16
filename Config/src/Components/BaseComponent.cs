using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components;

public abstract class BaseComponent : IComponent
{
    public event Action? Updated;

    private readonly List<IComponent> _children = [];
    public IEnumerable<IComponent> Children
    {
        get => _children.AsReadOnly();
        set => SetChildren(value);
    }

    public IComponent? Parent { get; set; }

    public FancyNode Node { get; } = new();
    RenderableNode IComponent.Node => Node;

    protected virtual Node ChildHost => Node;

    public virtual void AddChild(IComponent child, bool insert = true, bool update = true)
    {
        _children.Add(child);
        child.Updated += Updated;
        child.Parent = this;

        if (insert)
            ChildHost.InsertChild(child.Node, ChildHost.ChildCount);

        if (update)
            Updated?.Invoke();
    }

    public virtual void RemoveChild(IComponent child, bool update = true)
    {
        _children.Remove(child);
        child.Updated -= Updated;
        child.Parent = null;

        // This won't do anything if the node isn't a child of ChildHost
        ChildHost.RemoveChild(child.Node);

        if (update)
            Updated?.Invoke();
    }

    public virtual void SetChildren(
        IEnumerable<IComponent> children,
        bool insert = true,
        bool update = true
    )
    {
        foreach (var child in _children)
        {
            child.Updated -= Updated;
            ChildHost.RemoveChild(child.Node);
        }

        _children.Clear();

        foreach (var child in children)
            AddChild(child, insert, false);

        if (update)
            Updated?.Invoke();
    }
}

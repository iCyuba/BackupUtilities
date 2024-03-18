using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;

namespace BackupUtilities.Config.Components;

public abstract class BaseComponent : IComponent
{
    protected virtual IEnumerable<IComponent> SubComponents { get; } = [];

    public abstract RenderableNode Node { get; }

    protected IView? View { get; private set; }

    public virtual void Register(IView view)
    {
        if (view != this)
            View = view;

        if (this is IView && view != this)
            return;

        foreach (var component in SubComponents)
            component.Register(view);
    }

    public virtual void Unregister()
    {
        View = null;

        if (this is IView)
            return;

        foreach (var component in SubComponents)
            component.Unregister();
    }
}

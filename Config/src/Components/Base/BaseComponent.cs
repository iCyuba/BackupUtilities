using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;

namespace BackupUtilities.Config.Components.Base;

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

    /// <summary>
    /// Update the style of the component
    /// </summary>
    protected virtual void UpdateStyle() { }

    private readonly Dictionary<BaseModal, Action> _callbacks = [];

    /// <summary>
    /// Open a modal
    /// </summary>
    /// <param name="modal">The modal to open</param>
    protected void OpenModal(BaseModal modal)
    {
        Node.InsertChild(modal.Node, 0);

        var view = View;

        if (this is IView v)
            view = v;

        // Preserve focus
        IInteractive? focus = null;
        if (view != null)
        {
            modal.Register(view);
            focus = view.Active;
            view.Focus(modal);
        }

        _callbacks[modal] = Callback;
        modal.Closed += Callback;

        return;

        void Callback() => CloseModal(modal, focus);
    }

    private void CloseModal(BaseModal modal, IInteractive? focus)
    {
        Node.RemoveChild(modal.Node);
        modal.Unregister();

        var view = View;

        if (this is IView v)
            view = v;

        if (focus != null && view != null)
            view.Focus(focus);

        // Unregister the callback
        modal.Closed -= _callbacks[modal];
    }
}

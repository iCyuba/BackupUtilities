using System.Runtime.InteropServices;
using BackupUtilities.Config.Yoga.Interop;

namespace BackupUtilities.Config.Yoga;

/// <summary>
/// Node for the Yoga layout system.
///
/// <br/>
///
/// This is simply a wrapper around the C API.
/// But please do not use the C API directly, as some additional logic is handled here!
/// (For example, changing the children using the C api would not get detected by the C# wrapper)
/// </summary>
/// <param name="handle"></param>
public unsafe partial class Node(Config config)
    : YogaHandle(Methods.YGNodeNewWithConfig(config.Handle))
{
    /// <summary>
    /// The children of this node. solely for reference counting.
    /// </summary>
    private List<Node> _children = [];

    /// <summary>
    /// An owner is used to identify the YogaTree that a Node belongs to.
    /// This will return the parent of the Node when a Node only belongs to
    /// one YogaTree or null when the Node is shared between two or more YogaTrees.
    /// </summary>
    public Node? Owner { get; private set; }

    /// <summary>
    /// Alias of owner... Why? To match the C api
    /// </summary>
    public Node? Parent => Owner;

    private Config _config = config;

    private bool _disposed = false;

    // This is the same as the C "YGNodeNew" function, but it sets the config to the C# reference of the default one.
    public Node()
        : this(Config.Default) { }

    ~Node()
    {
        // Prevent finalization if the node has been freed
        if (!_disposed)
            Free();
    }

    public Node Clone() => throw new NotImplementedException();

    private void PreFinalize()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        _children.Clear();
        Owner = null;

        _disposed = true;
    }

    public void Free()
    {
        PreFinalize();

        Methods.YGNodeFree(Handle);
    }

    public void FreeRecursive()
    {
        PreFinalize();

        Methods.YGNodeFreeRecursive(Handle);
    }

    public void NodeFinalize()
    {
        PreFinalize();

        Methods.YGNodeFinalize(Handle);
    }

    public virtual void Reset()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        _children.Clear();
        Owner = null;

        Methods.YGNodeReset(Handle);
    }

    public void CalculateLayout(
        float width,
        float height,
        YGDirection direction = YGDirection.YGDirectionLTR
    ) => Methods.YGNodeCalculateLayout(Handle, width, height, direction);

    public bool HasNewLayout
    {
        get => Methods.YGNodeGetHasNewLayout(Handle);
        set => Methods.YGNodeSetHasNewLayout(Handle, value);
    }

    public bool IsDirty => Methods.YGNodeIsDirty(Handle);

    public void MarkDirty()
    {
        if (_measureFuncInternal == null)
            throw new InvalidOperationException(
                "Only leaf nodes with custom measure functions should manually mark themselves as dirty"
            );

        Methods.YGNodeMarkDirty(Handle);
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void DirtiedFuncInternalDelegate(void* node);

    // This must be referenced to prevent the delegate from being garbage collected.
    private DirtiedFuncInternalDelegate? _dirtiedFuncInternal;
    private Action<Node>? _dirtiedFunc;

    public Action<Node>? DirtiedFunc
    {
        get => _dirtiedFunc;
        set
        {
            _dirtiedFunc = value;

            if (value == null)
            {
                Methods.YGNodeSetDirtiedFunc(Handle, null);
                return;
            }

            // If the function had been set previously, the internal delegate can be reused.
            if (_dirtiedFuncInternal != null)
                return;

            _dirtiedFuncInternal = (_) => _dirtiedFunc!(this);
            Methods.YGNodeSetDirtiedFunc(
                Handle,
                (delegate* unmanaged[Cdecl]<void*, void>)
                    Marshal.GetFunctionPointerForDelegate(_dirtiedFuncInternal)
            );
        }
    }

    public void InsertChild(Node child, int index)
    {
        if (child.Owner != null)
            throw new InvalidOperationException(
                "Child already has an owner, it must be removed first."
            );

        if (_measureFuncInternal != null)
            throw new InvalidOperationException(
                "Cannot add child: Nodes with measure functions cannot have children."
            );

        _children.Insert(index, child);
        child.Owner = this;

        Methods.YGNodeInsertChild(Handle, child.Handle, (uint)index);
    }

    public void SwapChild(Node child, int index)
    {
        // The C function doesn't seem to reset the owner of the old child. So it's not done here either.
        _children[index] = child;
        child.Owner = this;

        // Unlike in InsertChild, there is no check being done here, for some reason.

        Methods.YGNodeSwapChild(Handle, child.Handle, (uint)index);
    }

    public void RemoveChild(Node child)
    {
        if (_children.Count == 0)
            return;

        // Only reset the owner, when it's this node
        if (Owner == this)
            child.Owner = null;

        // Remove the child from the list
        _children.Remove(child);

        Methods.YGNodeRemoveChild(Handle, child.Handle);
    }

    public void RemoveAllChildren()
    {
        if (_children.Count == 0)
            return;

        // If this node is the owner of the children (Yoga only checks for the first one),
        // it should be set to null.
        if (_children[0].Owner == this)
            foreach (Node child in _children)
                child.Owner = null;

        // Clear the children list
        _children.Clear();

        Methods.YGNodeRemoveAllChildren(Handle);
    }

    public void SetChildren(IEnumerable<Node> children)
    {
        // Set every old child's owner to null, unless it's in the new children list
        foreach (var child in _children.Where(child => !children.Contains(child)))
            child.Owner = null;

        // Store the children for reference counting
        _children = children.ToList();

        // Can't use LINQ when working with pointers ig
        void*[] handles = new void*[_children.Count];

        for (int i = 0; i < _children.Count; i++)
        {
            Node child = _children[i];

            child.Owner = this;
            handles[i] = child.Handle;
        }

        fixed (void** handlesPtr = &handles[0])
            Methods.YGNodeSetChildren(Handle, handlesPtr, (uint)_children.Count);
    }

    public Node? GetChild(int index)
    {
        if (index < 0 || index >= _children.Count)
            return null;

        return _children[index];
    }

    public Config Config
    {
        get => _config;
        set
        {
            if (_config == null)
                throw new InvalidOperationException(
                    "Cannot set the config of a node that is not owned by a YogaTree."
                );

            if (_config.UseWebDefaults != value.UseWebDefaults)
                throw new InvalidOperationException(
                    "UseWebDefaults may not be changed after constructing a Node"
                );

            _config = value;

            Methods.YGNodeSetConfig(Handle, value == null ? null : value.Handle);
        }
    }

    public int ChildCount => _children.Count;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate YGSize MeasureFuncInternalDelegate(
        void* node,
        float width,
        YGMeasureMode widthMode,
        float height,
        YGMeasureMode heightMode
    );

    private MeasureFuncInternalDelegate? _measureFuncInternal;
    private Func<Node, float, YGMeasureMode, float, YGMeasureMode, YGSize>? _measureFunc;

    public Func<Node, float, YGMeasureMode, float, YGMeasureMode, YGSize>? MeasureFunc
    {
        get => _measureFunc;
        set
        {
            if (_children.Count > 0)
                throw new InvalidOperationException(
                    "Cannot set measure function: Nodes with measure functions cannot have children."
                );

            _measureFunc = value;

            if (value == null)
            {
                Methods.YGNodeSetMeasureFunc(Handle, null);
                return;
            }

            // Reuse the internal function, when available
            if (_measureFuncInternal != null)
                return;

            _measureFuncInternal = (_, w, wMode, h, hMode) =>
                _measureFunc!(this, w, wMode, h, hMode);

            Methods.YGNodeSetMeasureFunc(
                Handle,
                (delegate* unmanaged[Cdecl]<
                    void*,
                    float,
                    YGMeasureMode,
                    float,
                    YGMeasureMode,
                    YGSize>)
                    Marshal.GetFunctionPointerForDelegate(_measureFuncInternal)
            );
        }
    }

    public bool HasMeasureFunc => Methods.YGNodeHasMeasureFunc(Handle);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate float BaselineFuncInternalDelegate(void* node, float width, float height);

    private BaselineFuncInternalDelegate? _baselineFuncInternal;
    private Func<Node, float, float, float>? _baselineFunc;

    public Func<Node, float, float, float>? BaselineFunc
    {
        get => _baselineFunc;
        set
        {
            _baselineFunc = value;

            if (value == null)
            {
                Methods.YGNodeSetBaselineFunc(Handle, null);
                return;
            }

            // Reuse the internal function, when available
            if (_baselineFuncInternal != null)
                return;

            _baselineFuncInternal = (_, w, h) => _baselineFunc!(this, w, h);
            Methods.YGNodeSetBaselineFunc(
                Handle,
                (delegate* unmanaged[Cdecl]<void*, float, float, float>)
                    Marshal.GetFunctionPointerForDelegate(_baselineFuncInternal)
            );
        }
    }

    public bool HasBaselineFunc => Methods.YGNodeHasBaselineFunc(Handle);

    public bool IsReferenceBaseline
    {
        get => Methods.YGNodeIsReferenceBaseline(Handle);
        set => Methods.YGNodeSetIsReferenceBaseline(Handle, value);
    }

    public YGNodeType Type
    {
        get => Methods.YGNodeGetNodeType(Handle);
        set => Methods.YGNodeSetNodeType(Handle, value);
    }

    public bool AlwaysFormsContainingBlock
    {
        get => Methods.YGNodeGetAlwaysFormsContainingBlock(Handle);
        set => Methods.YGNodeSetAlwaysFormsContainingBlock(Handle, value);
    }
}

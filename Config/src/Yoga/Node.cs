using System.Runtime.InteropServices;
using BackupUtilities.Config.Yoga.Interop;

namespace BackupUtilities.Config.Yoga;

public unsafe partial class Node(void* handle) : YogaHandle(handle)
{
    public Node(Config config)
        : this(Methods.YGNodeNewWithConfig(config.Handle)) { }

    public Node()
        : this(Methods.YGNodeNew()) { }

    public Node Clone() => new(Methods.YGNodeClone(Handle));

    public virtual void Free() => Methods.YGNodeFree(Handle);

    public virtual void FreeRecursive() => Methods.YGNodeFreeRecursive(Handle);

    public virtual void NodeFinalize() => Methods.YGNodeFinalize(Handle);

    public virtual void Reset() => Methods.YGNodeReset(Handle);

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

    public void MarkDirty() => Methods.YGNodeMarkDirty(Handle);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void DirtiedFuncInternalDelegate(void* node);

    // This must be referenced to prevent the delegate from being garbage collected.
    private DirtiedFuncInternalDelegate? _dirtiedFuncInternal;
    private Action? _dirtiedFunc;

    public Action? DirtiedFunc
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

            // If the function had been set previously, we can reuse the internal delegate, and just update the dirtied function reference.
            if (_dirtiedFuncInternal != null)
                return;

            _dirtiedFuncInternal = (_) => _dirtiedFunc!();
            Methods.YGNodeSetDirtiedFunc(
                Handle,
                (delegate* unmanaged[Cdecl]<void*, void>)
                    Marshal.GetFunctionPointerForDelegate(_dirtiedFuncInternal)
            );
        }
    }

    public void InsertChild(Node child, uint index) =>
        Methods.YGNodeInsertChild(Handle, child.Handle, index);

    public void SwapChild(Node child, uint index) =>
        Methods.YGNodeSwapChild(Handle, child.Handle, index);

    public void RemoveChild(Node child) => Methods.YGNodeRemoveChild(Handle, child.Handle);

    public void RemoveAllChildren() => Methods.YGNodeRemoveAllChildren(Handle);

    public void SetChildren(IEnumerable<Node> children)
    {
        int count = children.Count();

        // Can't use LINQ when working with pointers ig
        void*[] handles = new void*[count];

        for (int i = 0; i < children.Count(); i++)
            handles[i] = children.ElementAt(i).Handle;

        fixed (void** handlesPtr = &handles[0])
            Methods.YGNodeSetChildren(Handle, handlesPtr, (uint)count);
    }

    public Node? GetChild(uint index)
    {
        void* child = Methods.YGNodeGetChild(Handle, index);
        return child == null ? null : new Node(child);
    }

    public nuint ChildCount => Methods.YGNodeGetChildCount(Handle);

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

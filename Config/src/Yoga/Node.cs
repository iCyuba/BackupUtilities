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

    // public Action DirtifiedFunc {
    //     get => Methods.YGNodeGetDirtiedFunc(Handle);
    //     set => Methods.YGNodeSetDirtiedFunc(Handle, value);
    // }

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

    // public void SetMeasureFunc(Func<void*, float, YGMeasureMode, float, YGMeasureMode, YGSize> func)
    // {
    //     delegate* unmanaged[Cdecl]<void*, float, YGMeasureMode, float, YGMeasureMode, YGSize> f = &MeasureFuncInternal;
    // }

    public bool HasMeasureFunc => Methods.YGNodeHasMeasureFunc(Handle);

    // public void SetBaselineFunc(Func<float, float, float> func)
    //     => Methods.YGNodeSetBaselineFunc(Handle, func);

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

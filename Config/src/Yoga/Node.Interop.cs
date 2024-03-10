using System.Runtime.InteropServices;

namespace BackupUtilities.Config.Yoga;

// https://github.com/facebook/yoga/blob/753b3199774d3ead5a2d202f06d14b9bdcbe4e29/yoga/YGNode.h
public unsafe partial class Node
{
    /**
     * Heap allocates and returns a new Yoga node using Yoga settings.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void* YGNodeNew();

    /**
     * Heap allocates and returns a new Yoga node, with customized settings.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void* YGNodeNewWithConfig(void* config);

    /**
     * Returns a mutable copy of an existing node, with the same context and
     * children, but no owner set. Does not call the function set by
     * YGConfigSetCloneNodeFunc().
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void* YGNodeClone(void* node);

    /**
     * Frees the Yoga node, disconnecting it from its owner and children.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeFree(void* node);

    /**
     * Frees the subtree of Yoga nodes rooted at the given node.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeFreeRecursive(void* node);

    /**
     * Frees the Yoga node without disconnecting it from its owner or children.
     * Allows garbage collecting Yoga nodes in parallel when the entire tree is
     * unrechable.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeFinalize(void* node);

    /**
     * Resets the node to its default state.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeReset(void* node);

    /**
     * Calculates the layout of the tree rooted at the given node.
     *
     * Layout results may be read after calling YGNodeCalculateLayout() using
     * functions like YGNodeLayoutGetLeft(), YGNodeLayoutGetTop(), etc.
     *
     * YGNodeGetHasNewLayout() may be read to know if the layout of the node or its
     * subtrees may have changed since the last time YGNodeCalculate() was called.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeCalculateLayout(
        void* node,
        float availableWidth,
        float availableHeight,
        Direction ownerDirection
    );

    /**
     * Whether the given node may have new layout results. Must be reset by calling
     * YGNodeSetHasNewLayout().
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static partial bool YGNodeGetHasNewLayout(void* node);

    /**
     * Sets whether a nodes layout is considered new.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeSetHasNewLayout(
        void* node,
        [MarshalAs(UnmanagedType.I1)] bool hasNewLayout
    );

    /**
     * Whether the node's layout results are dirty due to it or its children
     * changing.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static partial bool YGNodeIsDirty(void* node);

    /**
    * Marks a node with custom measure function as dirty.
    */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeMarkDirty(void* node);

    /**
     * Inserts a child node at the given index.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeInsertChild(void* node, void* child, nuint index);

    /**
     * Replaces the child node at a given index with a new one.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeSwapChild(void* node, void* child, nuint index);

    /**
     * Removes the given child node.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeRemoveChild(void* node, void* child);

    /**
     * Removes all children nodes.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeRemoveAllChildren(void* node);

    /**
     * Sets children according to the given list of nodes.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeSetChildren(void* owner, void** children, nuint count);

    /**
     * Get the child node at a given index.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void* YGNodeGetChild(void* node, nuint index);

    /**
     * The number of child nodes.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial nuint YGNodeGetChildCount(void* node);

    /**
     * Get the parent/owner currently set for a node.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void* YGNodeGetOwner(void* node);

    /**
     * Get the parent/owner currently set for a node.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void* YGNodeGetParent(void* node);

    /**
     * Set a new config for the node after creation.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeSetConfig(void* node, void* config);

    /**
     * Get the config currently set on the node.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void* YGNodeGetConfig(void* node);

    /**
     * Sets extra data on the Yoga node which may be read from during callbacks.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeSetContext(void* node, void* context);

    /**
     * Returns the context or NULL if no context has been set.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void* YGNodeGetContext(void* node);

    /**
     * Sets this node should be considered the reference baseline among siblings.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeSetIsReferenceBaseline(
        void* node,
        [MarshalAs(UnmanagedType.I1)] bool isReferenceBaseline
    );

    /**
     * Whether this node is set as the reference baseline.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static partial bool YGNodeIsReferenceBaseline(void* node);

    /**
     * Sets whether a leaf node's layout results may be truncated during layout
     * rounding.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeSetNodeType(void* node, NodeType nodeType);

    /**
     * Wwhether a leaf node's layout results may be truncated during layout
     * rounding.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial NodeType YGNodeGetNodeType(void* node);

    /**
     * Make it so that this node will always form a containing block for any
     * descendant nodes. This is useful for when a node has a property outside of
     * of Yoga that will form a containing block. For example, transforms or some of
     * the others listed in
     * https://developer.mozilla.org/en-US/docs/Web/CSS/Containing_block
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeSetAlwaysFormsContainingBlock(
        void* node,
        [MarshalAs(UnmanagedType.I1)] bool alwaysFormsContainingBlock
    );

    /**
     * Whether the node will always form a containing block for any descendant. This
     * can happen in situation where the client implements something like a
     * transform that can affect containing blocks but is not handled by Yoga
     * directly.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static partial bool YGNodeGetAlwaysFormsContainingBlock(void* node);
}

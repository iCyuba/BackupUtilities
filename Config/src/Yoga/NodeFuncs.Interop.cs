using System.Runtime.InteropServices;

namespace BackupUtilities.Config.Yoga;

// https://github.com/facebook/yoga/blob/753b3199774d3ead5a2d202f06d14b9bdcbe4e29/yoga/YGNode.h
// (I chose to separate this into its own file for clarity)
public unsafe partial class Node
{
    delegate void YGDirtiedFunc(void* node);

    /**
     * Called when a change is made to the Yoga tree which dirties this node.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeSetDirtiedFunc(void* node, YGDirtiedFunc? dirtiedFunc);

    /**
     * Returns a dirtied func if set.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial YGDirtiedFunc YGNodeGetDirtiedFunc(void* node);

    /**
     * Returns the computed dimensions of the node, following the contraints of
     * `widthMode` and `heightMode`:
     *
     * YGMeasureModeUndefined: The parent has not imposed any constraint on the
     * child. It can be whatever size it wants.
     *
     * YGMeasureModeAtMost: The child can be as large as it wants up to the
     * specified size.
     *
     * YGMeasureModeExactly: The parent has determined an exact size for the
     * child. The child is going to be given those bounds regardless of how big it
     * wants to be.
     *
     * @returns the size of the leaf node, measured under the given contraints.
     */
    delegate Size YGMeasureFunc(
        void* node,
        float width,
        MeasureMode widthMode,
        float height,
        MeasureMode heightMode
    );

    /**
     * Allows providing custom measurements for a Yoga leaf node (usually for
     * measuring text). YGNodeMarkDirty() must be set if content effecting the
     * measurements of the node changes.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeSetMeasureFunc(void* node, YGMeasureFunc? measureFunc);

    /**
     * Whether a measure function is set.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static partial bool YGNodeHasMeasureFunc(void* node);

    /**
     * @returns a defined offet to baseline (ascent).
     */
    delegate float YGBaselineFunc(void* node, float width, float height);

    /**
     * Set a custom function for determining the text baseline for use in baseline
     * alignment.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeSetBaselineFunc(void* node, YGBaselineFunc? baselineFunc);

    /**
     * Whether a baseline function is set.
     */
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static partial bool YGNodeHasBaselineFunc(void* node);
}

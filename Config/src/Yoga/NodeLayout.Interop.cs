using System.Runtime.InteropServices;

namespace BackupUtilities.Config.Yoga;

// https://github.com/facebook/yoga/blob/c46ea9c6f5a4d11f6ee8eb45d428acb024cca81f/yoga/YGNodeLayout.h
public unsafe partial class Node
{
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial float YGNodeLayoutGetLeft(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial float YGNodeLayoutGetTop(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial float YGNodeLayoutGetRight(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial float YGNodeLayoutGetBottom(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial float YGNodeLayoutGetWidth(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial float YGNodeLayoutGetHeight(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Direction YGNodeLayoutGetDirection(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static partial bool YGNodeLayoutGetHadOverflow(void* node);

    // Get the computed values for these nodes after performing layout. If they were
    // set using point values then the returned value will be the same as
    // YGNodeStyleGetXXX. However if they were set using a percentage value then the
    // returned value is the computed value used during layout.
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial float YGNodeLayoutGetMargin(void* node, Edge edge);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial float YGNodeLayoutGetBorder(void* node, Edge edge);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial float YGNodeLayoutGetPadding(void* node, Edge edge);
}

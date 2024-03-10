using System.Runtime.InteropServices;

namespace BackupUtilities.Config.Yoga;

// https://github.com/facebook/yoga/blob/28975a6053c73ead2e4872905b9d5c14b6338865/yoga/YGConfig.h
public unsafe partial class Config : Base
{
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void* YGConfigNew();

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGConfigFree(void* config);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void* YGConfigGetDefault();

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGConfigSetUseWebDefaults(
        void* config,
        [MarshalAs(UnmanagedType.I1)] bool enabled
    );

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static partial bool YGConfigGetUseWebDefaults(void* config);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGConfigSetPointScaleFactor(void* config, float pixelsInPoint);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial float YGConfigGetPointScaleFactor(void* config);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGConfigSetErrata(void* config, Errata errata);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Errata YGConfigGetErrata(void* config);

    // Honestly not sure how this should be implemented in C#, but adding it for completeness
    delegate int YGLogger(void* config, void* node, LogLevel level, sbyte* format, IntPtr args);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGConfigSetLogger(void* config, YGLogger logger);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGConfigSetContext(void* config, void* context);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void* YGConfigGetContext(void* config);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    private static partial bool YGConfigIsExperimentalFeatureEnabled(
        void* config,
        ExperimentalFeature feature
    );

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGConfigSetExperimentalFeatureEnabled(
        void* config,
        ExperimentalFeature feature,
        [MarshalAs(UnmanagedType.I1)] bool enabled
    );

    delegate void* YGCloneNodeFunc(void* oldNode, void* owner, nuint childIndex);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGConfigSetCloneNodeFunc(void* config, YGCloneNodeFunc callback);
}

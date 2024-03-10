using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace BackupUtilities.Config.Yoga;

// https://github.com/facebook/yoga/blob/f90ad378ffcab1de7ed78583d38667717b377ca6/yoga/YGEnums.cpp
public static unsafe partial class YogaEnumExtensions
{
    [LibraryImport("yoga", EntryPoint = "YGAlignToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial sbyte* YGAlignToString(Align value);

    [LibraryImport("yoga", EntryPoint = "YGDimensionToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial sbyte* YGDimensionToString(Dimension value);

    [LibraryImport("yoga", EntryPoint = "YGDirectionToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial sbyte* YGDirectionToString(Direction value);

    [LibraryImport("yoga", EntryPoint = "YGDisplayToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial sbyte* YGDisplayToString(Display value);

    [LibraryImport("yoga", EntryPoint = "YGEdgeToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial sbyte* YGEdgeToString(Edge value);

    [LibraryImport("yoga", EntryPoint = "YGErrataToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial sbyte* YGErrataToString(Errata value);

    [LibraryImport("yoga", EntryPoint = "YGExperimentalFeatureToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial sbyte* YGExperimentalFeatureToString(ExperimentalFeature value);

    [LibraryImport("yoga", EntryPoint = "YGFlexDirectionToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial sbyte* YGFlexDirectionToString(FlexDirection value);

    [LibraryImport("yoga", EntryPoint = "YGGutterToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial sbyte* YGGutterToString(Gutter value);

    [LibraryImport("yoga", EntryPoint = "YGJustifyToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial sbyte* YGJustifyToString(Justify value);

    [LibraryImport("yoga", EntryPoint = "YGLogLevelToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial sbyte* YGLogLevelToString(LogLevel value);

    [LibraryImport("yoga", EntryPoint = "YGMeasureModeToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial sbyte* YGMeasureModeToString(MeasureMode value);

    [LibraryImport("yoga", EntryPoint = "YGNodeTypeToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial sbyte* YGNodeTypeToString(NodeType value);

    [LibraryImport("yoga", EntryPoint = "YGOverflowToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial sbyte* YGOverflowToString(Overflow value);

    [LibraryImport("yoga", EntryPoint = "YGPositionTypeToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial sbyte* YGPositionTypeToString(PositionType value);

    [LibraryImport("yoga", EntryPoint = "YGUnitToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial sbyte* YGUnitToString(Unit value);

    [LibraryImport("yoga", EntryPoint = "YGWrapToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial sbyte* YGWrapToString(Wrap value);
}

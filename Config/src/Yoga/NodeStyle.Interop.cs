using System.Runtime.InteropServices;

namespace BackupUtilities.Config.Yoga;

// https://github.com/facebook/yoga/blob/c46ea9c6f5a4d11f6ee8eb45d428acb024cca81f/yoga/YGNodeStyle.h
public unsafe partial class Node
{
    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeCopyStyle(void* dstNode, void* srcNode);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetDirection(void* node, Direction direction);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Direction YGNodeStyleGetDirection(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetFlexDirection(
        void* node,
        FlexDirection flexDirection
    );

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial FlexDirection YGNodeStyleGetFlexDirection(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetJustifyContent(void* node, Justify justifyContent);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Justify YGNodeStyleGetJustifyContent(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetAlignContent(void* node, Align alignContent);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Align YGNodeStyleGetAlignContent(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetAlignItems(void* node, Align alignItems);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Align YGNodeStyleGetAlignItems(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetAlignSelf(void* node, Align alignSelf);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Align YGNodeStyleGetAlignSelf(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetPositionType(void* node, PositionType positionType);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial PositionType YGNodeStyleGetPositionType(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetFlexWrap(void* node, Wrap flexWrap);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Wrap YGNodeStyleGetFlexWrap(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetOverflow(void* node, Overflow overflow);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Overflow YGNodeStyleGetOverflow(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetDisplay(void* node, Display display);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Display YGNodeStyleGetDisplay(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetFlex(void* node, float flex);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial float YGNodeStyleGetFlex(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetFlexGrow(void* node, float flexGrow);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial float YGNodeStyleGetFlexGrow(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetFlexShrink(void* node, float flexShrink);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial float YGNodeStyleGetFlexShrink(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetFlexBasis(void* node, float flexBasis);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetFlexBasisPercent(void* node, float flexBasis);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetFlexBasisAuto(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Length YGNodeStyleGetFlexBasis(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetPosition(void* node, Edge edge, float position);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetPositionPercent(
        void* node,
        Edge edge,
        float position
    );

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Length YGNodeStyleGetPosition(void* node, Edge edge);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetMargin(void* node, Edge edge, float margin);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetMarginPercent(void* node, Edge edge, float margin);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetMarginAuto(void* node, Edge edge);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Length YGNodeStyleGetMargin(void* node, Edge edge);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetPadding(void* node, Edge edge, float padding);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetPaddingPercent(void* node, Edge edge, float padding);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Length YGNodeStyleGetPadding(void* node, Edge edge);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetBorder(void* node, Edge edge, float border);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial float YGNodeStyleGetBorder(void* node, Edge edge);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetGap(void* node, Gutter gutter, float gapLength);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial float YGNodeStyleGetGap(void* node, Gutter gutter);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetWidth(void* node, float width);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetWidthPercent(void* node, float width);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetWidthAuto(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Length YGNodeStyleGetWidth(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetHeight(void* node, float height);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetHeightPercent(void* node, float height);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetHeightAuto(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Length YGNodeStyleGetHeight(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetMinWidth(void* node, float minWidth);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetMinWidthPercent(void* node, float minWidth);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Length YGNodeStyleGetMinWidth(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetMinHeight(void* node, float minHeight);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetMinHeightPercent(void* node, float minHeight);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Length YGNodeStyleGetMinHeight(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetMaxWidth(void* node, float maxWidth);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetMaxWidthPercent(void* node, float maxWidth);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Length YGNodeStyleGetMaxWidth(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetMaxHeight(void* node, float maxHeight);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetMaxHeightPercent(void* node, float maxHeight);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial Length YGNodeStyleGetMaxHeight(void* node);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial void YGNodeStyleSetAspectRatio(void* node, float aspectRatio);

    [LibraryImport("yoga", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    private static partial float YGNodeStyleGetAspectRatio(void* node);
}

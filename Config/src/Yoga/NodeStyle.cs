using BackupUtilities.Config.Yoga.Interop;

namespace BackupUtilities.Config.Yoga;

public unsafe partial class Node
{
    public void CopyStyle(Node source) => Methods.YGNodeCopyStyle(Handle, source.Handle);

    public YGDirection Direction
    {
        get => Methods.YGNodeStyleGetDirection(Handle);
        set => Methods.YGNodeStyleSetDirection(Handle, value);
    }

    public YGFlexDirection FlexDirection
    {
        get => Methods.YGNodeStyleGetFlexDirection(Handle);
        set => Methods.YGNodeStyleSetFlexDirection(Handle, value);
    }

    public YGJustify JustifyContent
    {
        get => Methods.YGNodeStyleGetJustifyContent(Handle);
        set => Methods.YGNodeStyleSetJustifyContent(Handle, value);
    }

    public YGAlign AlignContent
    {
        get => Methods.YGNodeStyleGetAlignContent(Handle);
        set => Methods.YGNodeStyleSetAlignContent(Handle, value);
    }

    public YGAlign AlignItems
    {
        get => Methods.YGNodeStyleGetAlignItems(Handle);
        set => Methods.YGNodeStyleSetAlignItems(Handle, value);
    }

    public YGAlign AlignSelf
    {
        get => Methods.YGNodeStyleGetAlignSelf(Handle);
        set => Methods.YGNodeStyleSetAlignSelf(Handle, value);
    }

    public YGPositionType PositionType
    {
        get => Methods.YGNodeStyleGetPositionType(Handle);
        set => Methods.YGNodeStyleSetPositionType(Handle, value);
    }

    public YGWrap FlexWrap
    {
        get => Methods.YGNodeStyleGetFlexWrap(Handle);
        set => Methods.YGNodeStyleSetFlexWrap(Handle, value);
    }

    public YGOverflow Overflow
    {
        get => Methods.YGNodeStyleGetOverflow(Handle);
        set => Methods.YGNodeStyleSetOverflow(Handle, value);
    }

    public YGDisplay Display
    {
        get => Methods.YGNodeStyleGetDisplay(Handle);
        set => Methods.YGNodeStyleSetDisplay(Handle, value);
    }

    public float Flex
    {
        get => Methods.YGNodeStyleGetFlex(Handle);
        set => Methods.YGNodeStyleSetFlex(Handle, value);
    }

    public float FlexGrow
    {
        get => Methods.YGNodeStyleGetFlexGrow(Handle);
        set => Methods.YGNodeStyleSetFlexGrow(Handle, value);
    }

    public float FlexShrink
    {
        get => Methods.YGNodeStyleGetFlexShrink(Handle);
        set => Methods.YGNodeStyleSetFlexShrink(Handle, value);
    }

    public Value FlexBasis => Value.FromYGValue(Methods.YGNodeStyleGetFlexBasis(Handle));

    public void SetFlexBasis(float value) => Methods.YGNodeStyleSetFlexBasis(Handle, value);

    public void SetFlexBasisPercent(float value) =>
        Methods.YGNodeStyleSetFlexBasisPercent(Handle, value);

    public void SetFlexBasisAuto() => Methods.YGNodeStyleSetFlexBasisAuto(Handle);

    public Value GetPosition(YGEdge edge) =>
        Value.FromYGValue(Methods.YGNodeStyleGetPosition(Handle, edge));

    public void SetPosition(YGEdge edge, float value) =>
        Methods.YGNodeStyleSetPosition(Handle, edge, value);

    public void SetPositionPercent(YGEdge edge, float value) =>
        Methods.YGNodeStyleSetPositionPercent(Handle, edge, value);

    public Value GetMargin(YGEdge edge) =>
        Value.FromYGValue(Methods.YGNodeStyleGetMargin(Handle, edge));

    public void SetMargin(YGEdge edge, float value) =>
        Methods.YGNodeStyleSetMargin(Handle, edge, value);

    public void SetMarginPercent(YGEdge edge, float value) =>
        Methods.YGNodeStyleSetMarginPercent(Handle, edge, value);

    public void SetMarginAuto(YGEdge edge) => Methods.YGNodeStyleSetMarginAuto(Handle, edge);

    public Value GetPadding(YGEdge edge) =>
        Value.FromYGValue(Methods.YGNodeStyleGetPadding(Handle, edge));

    public void SetPadding(YGEdge edge, float value) =>
        Methods.YGNodeStyleSetPadding(Handle, edge, value);

    public void SetPaddingPercent(YGEdge edge, float value) =>
        Methods.YGNodeStyleSetPaddingPercent(Handle, edge, value);

    public float GetBorder(YGEdge edge) => Methods.YGNodeStyleGetBorder(Handle, edge);

    public void SetBorder(YGEdge edge, float value) =>
        Methods.YGNodeStyleSetBorder(Handle, edge, value);

    public float GetGap(YGGutter gutter) => Methods.YGNodeStyleGetGap(Handle, gutter);

    public void SetGap(YGGutter gutter, float value) =>
        Methods.YGNodeStyleSetGap(Handle, gutter, value);

    public Value Width => Value.FromYGValue(Methods.YGNodeStyleGetWidth(Handle));

    public void SetWidth(float value) => Methods.YGNodeStyleSetWidth(Handle, value);

    public void SetWidthPercent(float value) => Methods.YGNodeStyleSetWidthPercent(Handle, value);

    public void SetWidthAuto() => Methods.YGNodeStyleSetWidthAuto(Handle);

    public Value Height => Value.FromYGValue(Methods.YGNodeStyleGetHeight(Handle));

    public void SetHeight(float value) => Methods.YGNodeStyleSetHeight(Handle, value);

    public void SetHeightPercent(float value) => Methods.YGNodeStyleSetHeightPercent(Handle, value);

    public void SetHeightAuto() => Methods.YGNodeStyleSetHeightAuto(Handle);

    public Value MinWidth => Value.FromYGValue(Methods.YGNodeStyleGetMinWidth(Handle));

    public void SetMinWidth(float value) => Methods.YGNodeStyleSetMinWidth(Handle, value);

    public void SetMinWidthPercent(float value) =>
        Methods.YGNodeStyleSetMinWidthPercent(Handle, value);

    public Value MinHeight => Value.FromYGValue(Methods.YGNodeStyleGetMinHeight(Handle));

    public void SetMinHeight(float value) => Methods.YGNodeStyleSetMinHeight(Handle, value);

    public void SetMinHeightPercent(float value) =>
        Methods.YGNodeStyleSetMinHeightPercent(Handle, value);

    public Value MaxWidth => Value.FromYGValue(Methods.YGNodeStyleGetMaxWidth(Handle));

    public void SetMaxWidth(float value) => Methods.YGNodeStyleSetMaxWidth(Handle, value);

    public void SetMaxWidthPercent(float value) =>
        Methods.YGNodeStyleSetMaxWidthPercent(Handle, value);

    public Value MaxHeight => Value.FromYGValue(Methods.YGNodeStyleGetMaxHeight(Handle));

    public void SetMaxHeight(float value) => Methods.YGNodeStyleSetMaxHeight(Handle, value);

    public void SetMaxHeightPercent(float value) =>
        Methods.YGNodeStyleSetMaxHeightPercent(Handle, value);

    public float AspectRatio
    {
        get => Methods.YGNodeStyleGetAspectRatio(Handle);
        set => Methods.YGNodeStyleSetAspectRatio(Handle, value);
    }
}

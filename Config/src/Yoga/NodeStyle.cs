namespace BackupUtilities.Config.Yoga;

public unsafe partial class Node
{
    public void CopyStyle(Node source) => YGNodeCopyStyle(Handle, source.Handle);

    public Direction Direction
    {
        get => YGNodeStyleGetDirection(Handle);
        set => YGNodeStyleSetDirection(Handle, value);
    }

    public FlexDirection FlexDirection
    {
        get => YGNodeStyleGetFlexDirection(Handle);
        set => YGNodeStyleSetFlexDirection(Handle, value);
    }

    public Justify JustifyContent
    {
        get => YGNodeStyleGetJustifyContent(Handle);
        set => YGNodeStyleSetJustifyContent(Handle, value);
    }

    public Align AlignContent
    {
        get => YGNodeStyleGetAlignContent(Handle);
        set => YGNodeStyleSetAlignContent(Handle, value);
    }

    public Align AlignItems
    {
        get => YGNodeStyleGetAlignItems(Handle);
        set => YGNodeStyleSetAlignItems(Handle, value);
    }

    public Align AlignSelf
    {
        get => YGNodeStyleGetAlignSelf(Handle);
        set => YGNodeStyleSetAlignSelf(Handle, value);
    }

    public PositionType PositionType
    {
        get => YGNodeStyleGetPositionType(Handle);
        set => YGNodeStyleSetPositionType(Handle, value);
    }

    public Wrap FlexWrap
    {
        get => YGNodeStyleGetFlexWrap(Handle);
        set => YGNodeStyleSetFlexWrap(Handle, value);
    }

    public Overflow Overflow
    {
        get => YGNodeStyleGetOverflow(Handle);
        set => YGNodeStyleSetOverflow(Handle, value);
    }

    public Display Display
    {
        get => YGNodeStyleGetDisplay(Handle);
        set => YGNodeStyleSetDisplay(Handle, value);
    }

    public float Flex
    {
        get => YGNodeStyleGetFlex(Handle);
        set => YGNodeStyleSetFlex(Handle, value);
    }

    public float FlexGrow
    {
        get => YGNodeStyleGetFlexGrow(Handle);
        set => YGNodeStyleSetFlexGrow(Handle, value);
    }

    public float FlexShrink
    {
        get => YGNodeStyleGetFlexShrink(Handle);
        set => YGNodeStyleSetFlexShrink(Handle, value);
    }

    public Length FlexBasis
    {
        get => YGNodeStyleGetFlexBasis(Handle);
        set
        {
            switch (value.Unit)
            {
                case Unit.Point:
                    YGNodeStyleSetFlexBasis(Handle, value.Value);
                    break;
                case Unit.Percent:
                    YGNodeStyleSetFlexBasisPercent(Handle, value.Value);
                    break;
                case Unit.Auto:
                    YGNodeStyleSetFlexBasisAuto(Handle);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }

    public Length GetPosition(Edge edge) => YGNodeStyleGetPosition(Handle, edge);

    public void SetPosition(Edge edge, float value) => YGNodeStyleSetPosition(Handle, edge, value);

    public void SetPositionPercent(Edge edge, float value) =>
        YGNodeStyleSetPositionPercent(Handle, edge, value);

    public Length GetMargin(Edge edge) => YGNodeStyleGetMargin(Handle, edge);

    public void SetMargin(Edge edge, float value) => YGNodeStyleSetMargin(Handle, edge, value);

    public void SetMarginPercent(Edge edge, float value) =>
        YGNodeStyleSetMarginPercent(Handle, edge, value);

    public void SetMarginAuto(Edge edge) => YGNodeStyleSetMarginAuto(Handle, edge);

    public Length GetPadding(Edge edge) => YGNodeStyleGetPadding(Handle, edge);

    public void SetPadding(Edge edge, float value) => YGNodeStyleSetPadding(Handle, edge, value);

    public void SetPaddingPercent(Edge edge, float value) =>
        YGNodeStyleSetPaddingPercent(Handle, edge, value);

    public float GetBorder(Edge edge) => YGNodeStyleGetBorder(Handle, edge);

    public void SetBorder(Edge edge, float value) => YGNodeStyleSetBorder(Handle, edge, value);

    public float GetGap(Gutter gutter) => YGNodeStyleGetGap(Handle, gutter);

    public void SetGap(Gutter gutter, float value) => YGNodeStyleSetGap(Handle, gutter, value);

    public Length Width
    {
        get => YGNodeStyleGetWidth(Handle);
        set
        {
            switch (value.Unit)
            {
                case Unit.Point:
                    YGNodeStyleSetWidth(Handle, value.Value);
                    break;
                case Unit.Percent:
                    YGNodeStyleSetWidthPercent(Handle, value.Value);
                    break;
                case Unit.Auto:
                    YGNodeStyleSetWidthAuto(Handle);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }

    public Length Height
    {
        get => YGNodeStyleGetHeight(Handle);
        set
        {
            switch (value.Unit)
            {
                case Unit.Point:
                    YGNodeStyleSetHeight(Handle, value.Value);
                    break;
                case Unit.Percent:
                    YGNodeStyleSetHeightPercent(Handle, value.Value);
                    break;
                case Unit.Auto:
                    YGNodeStyleSetHeightAuto(Handle);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }

    public Length MinWidth
    {
        get => YGNodeStyleGetMinWidth(Handle);
        set
        {
            switch (value.Unit)
            {
                case Unit.Point:
                    YGNodeStyleSetMinWidth(Handle, value.Value);
                    break;
                case Unit.Percent:
                    YGNodeStyleSetMinWidthPercent(Handle, value.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }

    public Length MinHeight
    {
        get => YGNodeStyleGetMinHeight(Handle);
        set
        {
            switch (value.Unit)
            {
                case Unit.Point:
                    YGNodeStyleSetMinHeight(Handle, value.Value);
                    break;
                case Unit.Percent:
                    YGNodeStyleSetMinHeightPercent(Handle, value.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }

    public Length MaxWidth
    {
        get => YGNodeStyleGetMaxWidth(Handle);
        set
        {
            switch (value.Unit)
            {
                case Unit.Point:
                    YGNodeStyleSetMaxWidth(Handle, value.Value);
                    break;
                case Unit.Percent:
                    YGNodeStyleSetMaxWidthPercent(Handle, value.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }

    public Length MaxHeight
    {
        get => YGNodeStyleGetMaxHeight(Handle);
        set
        {
            switch (value.Unit)
            {
                case Unit.Point:
                    YGNodeStyleSetMaxHeight(Handle, value.Value);
                    break;
                case Unit.Percent:
                    YGNodeStyleSetMaxHeightPercent(Handle, value.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }

    public float AspectRatio
    {
        get => YGNodeStyleGetAspectRatio(Handle);
        set => YGNodeStyleSetAspectRatio(Handle, value);
    }
}

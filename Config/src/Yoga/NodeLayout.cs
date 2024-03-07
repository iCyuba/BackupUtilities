using BackupUtilities.Config.Yoga.Interop;

namespace BackupUtilities.Config.Yoga;

public unsafe partial class Node
{
    public float ComputedLeft => Methods.YGNodeLayoutGetLeft(Handle);
    public float ComputedRight => Methods.YGNodeLayoutGetRight(Handle);

    public float ComputedTop => Methods.YGNodeLayoutGetTop(Handle);
    public float ComputedBottom => Methods.YGNodeLayoutGetBottom(Handle);

    public float ComputedWidth => Methods.YGNodeLayoutGetWidth(Handle);
    public float ComputedHeight => Methods.YGNodeLayoutGetHeight(Handle);

    public Layout GetComputedLayout() =>
        new()
        {
            Left = ComputedLeft,
            Right = ComputedRight,

            Top = ComputedTop,
            Bottom = ComputedBottom,

            Width = ComputedWidth,
            Height = ComputedHeight,
        };

    public YGDirection ComputedDirection => Methods.YGNodeLayoutGetDirection(Handle);

    public bool HadOverflow => Methods.YGNodeLayoutGetHadOverflow(Handle);

    public float GetComputedMargin(YGEdge edge) => Methods.YGNodeLayoutGetMargin(Handle, edge);

    public float GetComputedPadding(YGEdge edge) => Methods.YGNodeLayoutGetPadding(Handle, edge);

    public float GetComputedBorder(YGEdge edge) => Methods.YGNodeLayoutGetBorder(Handle, edge);
}

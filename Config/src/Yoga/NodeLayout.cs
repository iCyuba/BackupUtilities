namespace BackupUtilities.Config.Yoga;

public unsafe partial class Node
{
    public float ComputedLeft => YGNodeLayoutGetLeft(Handle);
    public float ComputedRight => YGNodeLayoutGetRight(Handle);

    public float ComputedTop => YGNodeLayoutGetTop(Handle);
    public float ComputedBottom => YGNodeLayoutGetBottom(Handle);

    public float ComputedWidth => YGNodeLayoutGetWidth(Handle);
    public float ComputedHeight => YGNodeLayoutGetHeight(Handle);

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

    public Direction ComputedDirection => YGNodeLayoutGetDirection(Handle);

    public bool HadOverflow => YGNodeLayoutGetHadOverflow(Handle);

    public float GetComputedMargin(Edge edge) => YGNodeLayoutGetMargin(Handle, edge);

    public float GetComputedPadding(Edge edge) => YGNodeLayoutGetPadding(Handle, edge);

    public float GetComputedBorder(Edge edge) => YGNodeLayoutGetBorder(Handle, edge);
}

using BackupUtilities.Config.Yoga;
using BackupUtilities.Config.Yoga.Interop;

namespace BackupUtilities.Config;

internal static class Program
{
    private static void Main()
    {
        Node root =
            new()
            {
                JustifyContent = YGJustify.YGJustifyCenter,
                AlignItems = YGAlign.YGAlignFlexEnd,
                FlexDirection = YGFlexDirection.YGFlexDirectionColumn,
                Overflow = YGOverflow.YGOverflowScroll,
            };

        root.SetGap(YGGutter.YGGutterAll, 2);

        // this isn't actually setting the border...
        root.SetBorder(YGEdge.YGEdgeAll, 1);
        root.SetPadding(YGEdge.YGEdgeVertical, 1);
        root.SetPadding(YGEdge.YGEdgeHorizontal, 3);

        Node node1 =
            new()
            {
                FlexGrow = 1,
                JustifyContent = YGJustify.YGJustifySpaceEvenly,
                AlignItems = YGAlign.YGAlignCenter,
                FlexDirection = YGFlexDirection.YGFlexDirectionRow,
            };

        node1.SetWidthPercent(100);
        node1.SetPadding(YGEdge.YGEdgeAll, 1);

        Node subNode1 = new();
        subNode1.SetWidth(10);
        subNode1.SetHeight(5);

        Node subNode2 =
            new() { MeasureFunc = (_, w, wMode, h, hMode) => new() { width = 15, height = 5 } };

        node1.SetChildren([subNode1, subNode2]);

        Node node2 = new();
        node2.SetWidth(50);
        node2.SetHeightPercent(25);

        root.SetChildren([node1, node2]);

        ConsoleRenderer renderer = new(root);
        renderer.Render();

        // Freeing the nodes isn't necessary, but why not
        root.FreeRecursive();
    }
}

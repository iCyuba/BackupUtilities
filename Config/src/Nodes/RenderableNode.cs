using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Nodes;

public abstract class RenderableNode : Node
{
    /// <summary>
    /// The Yoga configuration for the console.
    /// </summary>
    private static readonly Yoga.Config _consoleConfig = new() { UseWebDefaults = true };

    /// <summary>
    /// Scroll offsets, when scrolling is enabled.
    /// </summary>
    public (int x, int y) ScrollOffsets { get; set; }

    /// <summary>
    /// Is the absolute position fixed? (Only works with position type absolute)
    /// </summary>
    public bool IsFixed { get; set; } = false;

    protected RenderableNode()
        : base(_consoleConfig) { }

    protected record struct RenderOutput(
        Character[,] Buffer,
        (int x, int y) Offsets,
        List<RenderableNode> Absolute
    );

    /// <summary>
    /// Render to a 2D string array, X and Y offsets, and a list of absolute nodes that need to be rendered separately.
    /// </summary>
    /// <returns>The rendered node</returns>
    protected virtual RenderOutput Render()
    {
        // Skip text nodes (this isn't proper OOP, but idk anymore)
        if (Type == NodeType.Text)
            return new(new Character[0, 0], (0, 0), []);

        List<RenderableNode> absolute = [];

        // Get the offsets and sizes for the output
        (int x, int y) offsets = (0, 0);
        (int width, int height) = ((int)ComputedWidth, (int)ComputedHeight);

        Queue<(RenderableNode, RenderOutput)> renders = [];

        // Get the max overflow
        (int overflowScreenX, int overflowScreenY) =
            Overflow == Overflow.Visible ? (Console.WindowWidth, Console.WindowHeight) : (0, 0);

        int overflowXPos = (int)ComputedWidth + (int)ComputedLeft + overflowScreenX;
        int overflowXNeg = (int)ComputedLeft - overflowScreenX;
        int overflowYPos = (int)ComputedHeight + (int)ComputedTop + overflowScreenY;
        int overflowYNeg = (int)ComputedTop - overflowScreenY;

        // Render the children
        for (int i = 0; i < ChildCount; i++)
        {
            if (GetChild(i) is not RenderableNode child || child.Display == Display.None)
                continue;

            if (child.PositionType == PositionType.Absolute)
            {
                absolute.Add(child);
                continue;
            }

            // If the child is offscreen, don't render it
            // This could make it so overflowing nodes that have a negative margin don't render,
            // but render costs for that are high and it's not a common use case

            bool childFixed = child.IsFixed && child.PositionType == PositionType.Absolute;
            (int scrollX, int scrollY) =
                childFixed || Overflow != Overflow.Scroll ? (0, 0) : ScrollOffsets;

            if (
                child.ComputedLeft - scrollX > overflowXPos
                || child.ComputedTop - scrollY > overflowYPos
                || child.ComputedLeft + child.ComputedWidth - scrollX < overflowXNeg
                || child.ComputedTop + child.ComputedHeight - scrollY < overflowYNeg
            )
                continue;

            RenderChild(child);
        }

        // If the position type isn't static, we can render absolutely positioned nodes here
        if (PositionType != PositionType.Static)
        {
            foreach (var child in absolute)
                RenderChild(child);
        }

        // Merge the child renders into the main buffer
        var buffer = new Character[height + offsets.y, width + offsets.x];

        // Merge the children
        while (renders.Count > 0)
        {
            var (child, render) = renders.Dequeue();
            bool childFixed = child.IsFixed && child.PositionType == PositionType.Absolute;

            (int scrollX, int scrollY) =
                childFixed || Overflow != Overflow.Scroll ? (0, 0) : ScrollOffsets;

            buffer.Merge(
                render.Buffer,
                (
                    (int)child.ComputedLeft - render.Offsets.x + offsets.x - scrollX,
                    (int)child.ComputedTop - render.Offsets.y + offsets.y - scrollY
                )
            );
        }

        return new(buffer, offsets, absolute);

        void RenderChild(RenderableNode child)
        {
            // Render the child and its children
            var render = child.Render();

            // Overflow: Visible -> compute the offsets and sizes
            if (Overflow == Overflow.Visible)
            {
                offsets.x = Math.Max(offsets.x, render.Offsets.x - (int)child.ComputedLeft);
                offsets.y = Math.Max(offsets.y, render.Offsets.y - (int)child.ComputedTop);

                width = Math.Max(
                    width,
                    render.Buffer.GetLength(1) - render.Offsets.x + (int)child.ComputedLeft
                );
                height = Math.Max(
                    height,
                    render.Buffer.GetLength(0) - render.Offsets.y + (int)child.ComputedTop
                );
            }

            // Add the absolute nodes, if any
            absolute.AddRange(render.Absolute);

            // Queue the render for merging
            renders.Enqueue((child, render));
        }
    }
}

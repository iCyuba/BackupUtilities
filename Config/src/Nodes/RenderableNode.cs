using System.Numerics;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Nodes;

public abstract class RenderableNode : Node
{
    /// <summary>
    /// The Yoga configuration for the console.
    /// </summary>
    private static readonly Yoga.Config ConsoleConfig = new() { UseWebDefaults = true };

    /// <summary>
    /// Scroll position, when scrolling is enabled.
    /// </summary>
    public Vector2 Scroll { get; set; }

    /// <summary>
    /// Is the absolute position fixed? (Only works with position type absolute)
    /// </summary>
    public bool IsFixed { get; set; } = false;

    private bool IsReallyFixed => IsFixed && PositionType == PositionType.Absolute;

    protected RenderableNode()
        : base(ConsoleConfig)
    {
        PositionType = PositionType.Static;
    }

    protected readonly record struct RenderBuffer(Character[,] Buffer, Vector2 Offsets)
    {
        public Vector2 Size => new(Buffer.GetLength(1), Buffer.GetLength(0));

        public static RenderBuffer Empty => new(new Character[0, 0], default);
    }

    protected record RenderOutput(RenderBuffer Normal, RenderBuffer Absolute);

    /// <summary>
    /// Render to a 2D string array, X and Y offsets, and a list of absolute nodes that need to be rendered separately.
    /// </summary>
    /// <returns>The rendered node</returns>
    protected virtual RenderOutput Render()
    {
        // Skip text nodes (this isn't proper OOP, but idk anymore)
        if (Type == NodeType.Text)
            return new(RenderBuffer.Empty, RenderBuffer.Empty);

        // Get the offsets and sizes for the output
        Vector2 offsets = default;
        Vector2 size = new(ComputedWidth, ComputedHeight);

        Queue<(RenderableNode, RenderOutput)> renders = [];

        // Render the children
        for (int i = 0; i < ChildCount; i++)
            if (GetChild(i) is RenderableNode child && child.Display != Display.None)
                RenderChild(child);

        // Merge the child renders into the main buffer
        var bufferSize = size + offsets;
        var buffer = new Character[(int)bufferSize.Y, (int)bufferSize.X];
        var absolute = new Character[(int)bufferSize.Y, (int)bufferSize.X];

        // Merge the children
        while (renders.Count > 0)
        {
            var (child, render) = renders.Dequeue();

            Vector2 pos = new(child.ComputedLeft, child.ComputedTop);

            var scroll = child.IsReallyFixed || Overflow != Overflow.Scroll ? default : Scroll;
            var renderOffsets = pos - render.Normal.Offsets + offsets - scroll;

            if (child.PositionType == PositionType.Absolute)
                absolute.Merge(render.Normal.Buffer, renderOffsets);
            else
                buffer.Merge(render.Normal.Buffer, renderOffsets);

            absolute.Merge(render.Absolute.Buffer, pos - render.Absolute.Offsets + offsets);
        }

        // If position isn't static, merge absolute nodes into the main buffer
        if (PositionType != PositionType.Static)
            buffer.Merge(absolute);

        RenderBuffer absoluteBuffer = new(absolute, offsets);

        if (Overflow != Overflow.Visible)
        {
            buffer = buffer.Expand(size, -offsets);
            offsets = default;
        }

        return new(new(buffer, offsets), absoluteBuffer);

        void RenderChild(RenderableNode child)
        {
            // Render the child and its children
            var render = child.Render();

            Vector2 pos = new(child.ComputedLeft, child.ComputedTop);

            // Compute the offsets and size
            offsets = Vector2.Max(
                offsets,
                Vector2.Max(render.Normal.Offsets, render.Absolute.Offsets) - pos
            );

            size = Vector2.Max(
                size,
                Vector2.Max(
                    render.Normal.Size - render.Normal.Offsets,
                    render.Absolute.Size - render.Absolute.Offsets
                ) + pos
            );

            // Queue the render for merging
            renders.Enqueue((child, render));
        }
    }
}

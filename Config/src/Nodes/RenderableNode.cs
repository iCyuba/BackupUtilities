using System.Numerics;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Nodes;

/// <summary>
/// Extension of Yoga's Node class that can be rendered to the console.
/// </summary>
public abstract class RenderableNode : Node
{
    /// <summary>
    /// The Yoga configuration for the console.
    /// </summary>
    private static readonly Yoga.Config ConsoleConfig = new() { UseWebDefaults = true };

    private Vector2 _scroll;

    /// <summary>
    /// Scroll position, when scrolling is enabled.
    /// </summary>
    public Vector2 Scroll
    {
        get => Overflow == Overflow.Scroll ? _scroll : default;
        set
        {
            _scroll = value;
            _scrollTo = null;
        }
    }

    private Node? _scrollTo;

    /// <summary>
    /// The node to which the scroll is locked to.
    /// </summary>
    public Node? ScrollTo
    {
        get => _scrollTo;
        set
        {
            _scrollTo = value;
            _scroll = default; // This will be set automatically during rendering
        }
    }

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
        Vector2 computed = new(ComputedWidth, ComputedHeight);
        var size = computed;

        Queue<(RenderableNode, RenderOutput)> renders = [];

        // Render the children
        for (int i = 0; i < ChildCount; i++)
            if (GetChild(i) is RenderableNode child && child.Display != Display.None)
                RenderChild(child);

        var bufferSize = size + offsets;
        var buffer = new Character[(int)bufferSize.Y, (int)bufferSize.X];
        var absolute = new Character[(int)bufferSize.Y, (int)bufferSize.X];

        // Get the correct scroll position
        if (_scrollTo != null)
            _scroll = new(
                _scrollTo.ComputedLeft + _scrollTo.ComputedWidth,
                _scrollTo.ComputedTop + _scrollTo.ComputedHeight
            );

        var scroll = Vector2.Max(Scroll - computed, default);

        // Merge the children
        while (renders.Count > 0)
        {
            var (child, render) = renders.Dequeue();

            Vector2 pos = new(child.ComputedLeft, child.ComputedTop);

            var po = pos + offsets;
            if (child.PositionType == PositionType.Absolute)
                absolute.Merge(render.Normal.Buffer, po - render.Normal.Offsets);
            else
            {
                buffer.Merge(render.Normal.Buffer, po - render.Normal.Offsets - scroll);

                absolute.Merge(render.Absolute.Buffer, po - render.Absolute.Offsets);
            }
        }

        // If position isn't static, merge absolute nodes into the main buffer
        if (PositionType != PositionType.Static)
            buffer.Merge(absolute);

        RenderBuffer absoluteBuffer = new(absolute, offsets);

        if (Overflow != Overflow.Visible)
        {
            buffer = buffer.Expand(computed, -offsets);
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

using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.ConsoleUI;

public abstract class RenderableNode : Node
{
    /// <summary>
    /// The Yoga configuration for the console.
    /// </summary>
    private static readonly Yoga.Config _consoleConfig = new() { UseWebDefaults = true };

    protected RenderableNode()
        : base(_consoleConfig) { }

    protected record struct RenderOutput(
        string[,] Output,
        (int x, int y) Offsets,
        List<RenderableNode> Absolute
    )
    {
        public static RenderOutput Empty => new(new string[0, 0], (0, 0), []);
    }

    /// <summary>
    /// Render to a 2D string array, X and Y offsets, and a list of absolute nodes that need to be rendered separately.
    /// </summary>
    /// <returns>The rendered node</returns>
    protected virtual RenderOutput Render()
    {
        // Skip text nodes (this isn't proper OOP, but idk anymore)
        if (Type == NodeType.Text)
            return RenderOutput.Empty;

        List<RenderableNode> absolute = [];

        // Get the offsets and sizes for the output
        (int x, int y) offsets = (0, 0);
        (int width, int height) = ((int)ComputedWidth, (int)ComputedHeight);

        Queue<(Node, RenderOutput)> renders = [];

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

            // Render the child and its children
            var render = child.Render();

            // Max the offsets and sizes
            offsets.x = Math.Max(offsets.x, render.Offsets.x - (int)child.ComputedLeft);
            offsets.y = Math.Max(offsets.y, render.Offsets.y - (int)child.ComputedTop);

            width = Math.Max(
                width,
                render.Output.GetLength(1) - render.Offsets.x + (int)child.ComputedLeft
            );
            height = Math.Max(
                height,
                render.Output.GetLength(0) - render.Offsets.y + (int)child.ComputedTop
            );

            // Add the absolute nodes, if any
            absolute.AddRange(render.Absolute);

            // Queue the render for merging
            renders.Enqueue((child, render));
        }

        // Merge the child renders into the main output
        string[,] output = new string[height + offsets.y, width + offsets.x];

        // Merge the children
        while (renders.Count > 0)
        {
            var (child, render) = renders.Dequeue();
            output.Merge(
                render.Output,
                (
                    (int)child.ComputedLeft - render.Offsets.x + offsets.x,
                    (int)child.ComputedTop - render.Offsets.y + offsets.y
                )
            );
        }

        return new(output, offsets, absolute);
    }
}

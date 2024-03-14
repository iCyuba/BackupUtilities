using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Nodes;

public class FancyNode : RenderableNode
{
    protected override RenderOutput Render()
    {
        var render = base.Render();

        // Border
        float left = GetComputedBorder(Edge.Left);
        float right = GetComputedBorder(Edge.Right);
        float top = GetComputedBorder(Edge.Top);
        float bottom = GetComputedBorder(Edge.Bottom);

        for (int x = 0; x < ComputedWidth; x++)
        for (int y = 0; y < ComputedHeight; y++)
            if (
                x - left < 0
                || x >= ComputedWidth - right
                || y - top < 0
                || y == ComputedHeight - bottom
            )
            {
                render.Buffer[y + render.Offsets.y, x + render.Offsets.x] = "█";
            }

        return render;
    }
}

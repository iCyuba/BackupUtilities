using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Nodes;

public class FancyNode : RenderableNode
{
    public Color? Color { get; set; }

    protected override RenderOutput Render()
    {
        var render = base.Render();

        // Border
        float left = GetComputedBorder(Edge.Left);
        float right = GetComputedBorder(Edge.Right);
        float top = GetComputedBorder(Edge.Top);
        float bottom = GetComputedBorder(Edge.Bottom);

        // Size
        float width = ComputedWidth;
        float height = ComputedHeight;

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            int xx = x + (int)render.Normal.Offsets.X;
            int yy = y + (int)render.Normal.Offsets.Y;

            render.Normal.Buffer[yy, xx].Foreground ??= Color;

            if ((top >= 1 || left >= 1) && x == 0 && y == 0)
                render.Normal.Buffer[yy, xx].Value = "";
            else if ((bottom >= 1 || right >= 1) && x == width - 1 && y == height - 1)
                render.Normal.Buffer[yy, xx].Value = "";
            else
            {
                render.Normal.Buffer[yy, xx].Value ??= " ";
                render.Normal.Buffer[yy, xx].Background ??= Color;
            }
        }

        return render;
    }
}

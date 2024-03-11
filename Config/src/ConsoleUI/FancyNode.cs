namespace BackupUtilities.Config.ConsoleUI;

public class FancyNode : RenderableNode
{
    public FancyNode()
        : base()
    {
        SetBorder(Yoga.Edge.All, 1);
    }

    protected override RenderOutput Render()
    {
        var render = base.Render();

        // Border
        for (int x = 0; x < ComputedWidth; x++)
        for (int y = 0; y < ComputedHeight; y++)
            if (x == 0 || x == ComputedWidth - 1 || y == 0 || y == ComputedHeight - 1)
                render.Output[y + render.Offsets.y, x + render.Offsets.x] = "█";

        return render;
    }
}

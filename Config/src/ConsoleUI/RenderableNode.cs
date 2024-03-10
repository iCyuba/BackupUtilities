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

    /// <summary>
    /// Renders the node to a 2D string array.
    /// </summary>
    /// <returns>The rendered node</returns>
    public abstract string[,] Render();
}

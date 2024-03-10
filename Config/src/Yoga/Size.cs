using BackupUtilities.Config.Yoga.Interop;

namespace BackupUtilities.Config.Yoga;

public record Size(float Width, float Height)
{
    public YGSize ToYGSize() => new() { width = Width, height = Height };
}

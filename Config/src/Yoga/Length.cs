namespace BackupUtilities.Config.Yoga;

public record struct Length(Unit Unit, float Value)
{
    public Length(float points)
        : this(Unit.Point, points) { }

    public static Length Zero => new(Unit.Point, 0);
    public static Length Auto => new(Unit.Auto, float.NaN);
    public static Length Undefined => new(Unit.Undefined, float.NaN);
}

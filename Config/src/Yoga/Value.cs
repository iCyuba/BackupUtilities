using BackupUtilities.Config.Yoga.Interop;

namespace BackupUtilities.Config.Yoga;

public struct Value()
{
    public YGUnit unit = YGUnit.YGUnitUndefined;
    public float value = 0;

    public YGValue ToYGValue() => new() { unit = unit, value = value };

    public static Value FromYGValue(YGValue value) =>
        new() { unit = value.unit, value = value.value };
}

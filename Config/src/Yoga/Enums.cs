namespace BackupUtilities.Config.Yoga;

public enum Align : uint
{
    Auto,
    FlexStart,
    Center,
    FlexEnd,
    Stretch,
    Baseline,
    SpaceBetween,
    SpaceAround,
    SpaceEvenly
}

public enum Dimension : uint
{
    Width,
    Height
}

public enum Direction : uint
{
    Inherit,
    LTR,
    RTL
}

public enum Display : uint
{
    Flex,
    None
}

public enum Edge : uint
{
    Left,
    Top,
    Right,
    Bottom,
    Start,
    End,
    Horizontal,
    Vertical,
    All
}

[Flags]
public enum Errata : uint
{
    None = 0,
    StretchFlexBasis = 1,
    AbsolutePositioningIncorrect = 2,
    AbsolutePercentAgainstInnerSize = 4,
    All = 2147483647,
    Classic = 2147483646
}

public enum ExperimentalFeature : uint
{
    WebFlexBasis
}

public enum FlexDirection : uint
{
    Column,
    ColumnReverse,
    Row,
    RowReverse
}

public enum Gutter : uint
{
    Column,
    Row,
    All
}

public enum Justify : uint
{
    FlexStart,
    Center,
    FlexEnd,
    SpaceBetween,
    SpaceAround,
    SpaceEvenly
}

public enum LogLevel : uint
{
    Error,
    Warn,
    Info,
    Debug,
    Verbose,
    Fatal
}

public enum MeasureMode : uint
{
    Undefined,
    Exactly,
    AtMost
}

public enum NodeType : uint
{
    Default,
    Text
}

public enum Overflow : uint
{
    Visible,
    Hidden,
    Scroll
}

public enum PositionType : uint
{
    Static,
    Relative,
    Absolute
}

public enum Unit : uint
{
    Undefined,
    Point,
    Percent,
    Auto
}

public enum Wrap : uint
{
    NoWrap,
    Wrap,
    WrapReverse
}

/// <summary>
/// Extension methods for enums in the Yoga library
/// </summary>
public static unsafe partial class YogaEnumExtensions
{
    public static string ToCSSString(this Align value) => new(YGAlignToString(value));

    public static string ToCSSString(this Dimension value) => new(YGDimensionToString(value));

    public static string ToCSSString(this Direction value) => new(YGDirectionToString(value));

    public static string ToCSSString(this Display value) => new(YGDisplayToString(value));

    public static string ToCSSString(this Edge value) => new(YGEdgeToString(value));

    public static string ToCSSString(this Errata value) => new(YGErrataToString(value));

    public static string ToCSSString(this ExperimentalFeature value) =>
        new(YGExperimentalFeatureToString(value));

    public static string ToCSSString(this FlexDirection value) =>
        new(YGFlexDirectionToString(value));

    public static string ToCSSString(this Gutter value) => new(YGGutterToString(value));

    public static string ToCSSString(this Justify value) => new(YGJustifyToString(value));

    public static string ToCSSString(this LogLevel value) => new(YGLogLevelToString(value));

    public static string ToCSSString(this MeasureMode value) => new(YGMeasureModeToString(value));

    public static string ToCSSString(this NodeType value) => new(YGNodeTypeToString(value));

    public static string ToCSSString(this Overflow value) => new(YGOverflowToString(value));

    public static string ToCSSString(this PositionType value) => new(YGPositionTypeToString(value));

    public static string ToCSSString(this Unit value) => new(YGUnitToString(value));

    public static string ToCSSString(this Wrap value) => new(YGWrapToString(value));
}

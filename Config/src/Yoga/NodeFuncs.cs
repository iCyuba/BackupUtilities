namespace BackupUtilities.Config.Yoga;

public unsafe partial class Node : Base
{
    // References to the delegate instances are kept to prevent them from being garbage collected.

    private YGDirtiedFunc? _dirtiedFuncInternal;
    private Action<Node>? _dirtiedFunc;

    public Action<Node>? DirtiedFunc
    {
        get => _dirtiedFunc;
        set
        {
            _dirtiedFunc = value;

            if (value == null)
            {
                YGNodeSetDirtiedFunc(Handle, null);
                return;
            }

            // If the function had been set previously, the internal delegate can be reused.
            if (_dirtiedFuncInternal != null)
                return;

            _dirtiedFuncInternal = (_) => _dirtiedFunc!(this);
            YGNodeSetDirtiedFunc(Handle, _dirtiedFuncInternal);
        }
    }

    private YGMeasureFunc? _measureFuncInternal;
    private Func<Node, float, MeasureMode, float, MeasureMode, Size>? _measureFunc;

    public Func<Node, float, MeasureMode, float, MeasureMode, Size>? MeasureFunc
    {
        get => _measureFunc;
        set
        {
            if (_children.Count > 0)
                throw new InvalidOperationException(
                    "Cannot set measure function: Nodes with measure functions cannot have children."
                );

            _measureFunc = value;

            if (value == null)
            {
                YGNodeSetMeasureFunc(Handle, null);
                return;
            }

            // Reuse the internal function, when available
            if (_measureFuncInternal != null)
                return;

            _measureFuncInternal = (_, w, wMode, h, hMode) =>
                _measureFunc!(this, w, wMode, h, hMode);

            YGNodeSetMeasureFunc(Handle, _measureFuncInternal);
        }
    }

    private YGBaselineFunc? _baselineFuncInternal;
    private Func<Node, float, float, float>? _baselineFunc;

    public Func<Node, float, float, float>? BaselineFunc
    {
        get => _baselineFunc;
        set
        {
            _baselineFunc = value;

            if (value == null)
            {
                YGNodeSetBaselineFunc(Handle, null);
                return;
            }

            // Reuse the internal function, when available
            if (_baselineFuncInternal != null)
                return;

            _baselineFuncInternal = (_, w, h) => _baselineFunc!(this, w, h);
            YGNodeSetBaselineFunc(Handle, _baselineFuncInternal);
        }
    }
}

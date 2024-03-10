using BackupUtilities.Config.Yoga.Interop;

namespace BackupUtilities.Config.Yoga;

public unsafe class Config : Base
{
    private bool _disposed = false;

    private Config(void* handle)
        : base(handle) { }

    public Config()
        : this(Methods.YGConfigNew()) { }

    ~Config()
    {
        if (!_disposed)
            Free();
    }

    public static Config Default { get; } = new(Methods.YGConfigGetDefault());

    public void Free()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        _disposed = true;
        Methods.YGConfigFree(Handle);
    }

    public bool ExperimentalFeatureWebFlexBasis
    {
        get => Methods.YGConfigIsExperimentalFeatureEnabled(Handle, 0);
        set => Methods.YGConfigSetExperimentalFeatureEnabled(Handle, 0, value);
    }

    public float PointScaleFactor
    {
        get => Methods.YGConfigGetPointScaleFactor(Handle);
        set => Methods.YGConfigSetPointScaleFactor(Handle, value);
    }

    public YGErrata Errata
    {
        get => Methods.YGConfigGetErrata(Handle);
        set => Methods.YGConfigSetErrata(Handle, value);
    }

    public bool UseWebDefaults
    {
        get => Methods.YGConfigGetUseWebDefaults(Handle);
        set => Methods.YGConfigSetUseWebDefaults(Handle, value);
    }
}

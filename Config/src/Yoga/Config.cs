using BackupUtilities.Config.Yoga.Interop;

namespace BackupUtilities.Config.Yoga;

public unsafe class Config(void* handle) : YogaHandle(handle)
{
    public Config()
        : this(Methods.YGConfigNew()) { }

    public void Free() => Methods.YGConfigFree(Handle);

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

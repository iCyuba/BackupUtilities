using System.Runtime.InteropServices;

namespace BackupUtilities.Config.Yoga;

/// <summary>
/// Set of configuration options. The configuration may be applied to
/// multiple nodes (i.e. a single global config), or can be applied more
/// granularly per-node.
/// </summary>
public unsafe partial class Config : Base
{
    private bool _disposed = false;

    private Config(void* handle)
        : base(handle) { }

    public Config()
        : this(YGConfigNew()) { }

    ~Config()
    {
        if (!_disposed)
            Free();
    }

    /// <summary>
    /// The default config values set by Yoga.
    /// </summary>
    public static Config Default { get; } = new(YGConfigGetDefault());

    /// <summary>
    /// Frees the associated Yoga configuration.
    /// </summary>
    public void Free()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        _disposed = true;
        YGConfigFree(Handle);
    }

    /// <summary>
    /// Yoga by default creates new nodes with style defaults different from flexbox
    /// on web (e.g. <see cref="FlexDirection.Column"/> and <see cref="PositionType.Relative"/>`YGPositionRelative`).
    /// `UseWebDefaults` instructs Yoga to instead use a default style consistent
    /// with the web.
    /// </summary>
    public bool UseWebDefaults
    {
        get => YGConfigGetUseWebDefaults(Handle);
        set => YGConfigSetUseWebDefaults(Handle, value);
    }

    /// <summary>
    /// Yoga will by deafult round final layout positions and dimensions to the
    /// nearst point. <see cref="PointScaleFactor"/> controls the density of the grid used for
    /// layout rounding (e.g. to round to the closest display pixel).<br/>
    /// <br/>
    /// May be set to 0.0f to avoid rounding the layout results.
    /// </summary>
    public float PointScaleFactor
    {
        get => YGConfigGetPointScaleFactor(Handle);
        set => YGConfigSetPointScaleFactor(Handle, value);
    }

    /// <summary>
    /// Configures how Yoga balances W3C conformance vs compatibility with layouts
    /// created against earlier versions of Yoga.<br/>
    /// <br/>
    /// By deafult Yoga will prioritize W3C conformance. <see cref="Errata"/> may be set to ask
    /// Yoga to produce specific incorrect behaviors. E.g. <see cref="Errata.StretchFlexBasis"/>.<br/>
    /// <br/>
    /// YGErrata is a bitmask, and multiple errata may be set at once. Predfined
    /// constants exist for convenience:
    /// 1. YGErrataNone: No errata
    /// 2. YGErrataClassic: Match layout behaviors of Yoga 1.x
    /// 3. YGErrataAll: Match layout behaviors of Yoga 1.x, including
    /// `UseLegacyStretchBehaviour`
    ///
    /// <list type="number">
    ///     <item>
    ///         <see cref="Errata.None" />: No errata
    ///     </item>
    ///     <item>
    ///         <see cref="Errata.Classic" />: Match layout behaviors of Yoga 1.x
    ///     </item>
    ///     <item>
    ///         <see cref="Errata.All" />: Match layout behaviors of Yoga 1.x, including UseLegacyStretchBehaviour
    ///     </item>
    /// </list>
    /// </summary>
    public Errata Errata
    {
        get => YGConfigGetErrata(Handle);
        set => YGConfigSetErrata(Handle, value);
    }

    /// <summary>
    /// Enable an experimental/unsupported feature in Yoga.
    /// </summary>
    public bool ExperimentalFeatureWebFlexBasis
    {
        get => YGConfigIsExperimentalFeatureEnabled(Handle, 0);
        set => YGConfigSetExperimentalFeatureEnabled(Handle, 0, value);
    }
}

namespace BackupUtilities.Config.Yoga;

public abstract unsafe class Base(void* handle)
{
    /** Reference to the underlying C handle. */
    public void* Handle { get; } = handle;
}

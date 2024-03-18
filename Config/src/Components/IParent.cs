namespace BackupUtilities.Config.Components;

public interface IParent<T> : IComponent
    where T : IComponent
{
    IEnumerable<T> Children { get; set; }
}

public interface IParent : IParent<IComponent> { }

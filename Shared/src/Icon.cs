using System.Reflection;

namespace BackupUtilities.Shared;

/// <summary>
/// An attribute to specify an icon for a property.
/// </summary>
/// <param name="value">The icon value.</param>
[AttributeUsage(AttributeTargets.Property)]
public class Icon(string value) : Attribute
{
    private readonly string _value = value;

    public static string GetIcon(PropertyInfo property)
    {
        var attribute = property.GetCustomAttribute<Icon>();
        return attribute?._value ?? "";
    }
}

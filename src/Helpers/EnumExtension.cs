using System.ComponentModel;
using System.Reflection;

namespace BackupUtility;

public static class EnumExtension
{
    /// <summary>
    /// Get the description of an enum value
    /// </summary>
    /// <param name="value">The enum value</param>
    /// <returns>The description of the enum value or a name if no description exists</returns>
    public static string GetDescription(this Enum value)
    {
        string name = value.ToString();

        // Find the description attribute from the field of the enum value
        var attribute = value.GetType().GetField(name)?.GetCustomAttribute<DescriptionAttribute>();

        // Return the description if it exists, otherwise return the name
        return attribute?.Description ?? name;
    }
}

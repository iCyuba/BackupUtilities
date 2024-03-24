using System.Linq.Expressions;
using System.Reflection;

namespace BackupUtilities.Config.Util;

/// <summary>
/// A reference to a property. Used for getting and setting properties of objects, without needing to know the name.
/// </summary>
/// <typeparam name="T">The type of the property.</typeparam>
// Inspired by: https://stackoverflow.com/a/73762917
public class PropertyRef<T>
{
    /// <summary>
    /// Name of the property.
    /// </summary>
    public string Name => _member.Member.Name;

    /// <summary>
    /// Property information of the property.
    /// </summary>
    public PropertyInfo Property => (PropertyInfo)_member.Member;

    /// <summary>
    /// Value of the property.
    /// </summary>
    public T Value
    {
        get => _getter();
        set => _setter(value);
    }

    private readonly MemberExpression _member;

    private readonly Func<T> _getter;
    private readonly Action<T> _setter;

    public PropertyRef(Expression<Func<T>> expression)
    {
        _member = (MemberExpression)expression.Body;

        _getter = expression.Compile();

        var param = Expression.Parameter(typeof(T));
        _setter = Expression.Lambda<Action<T>>(Expression.Assign(_member, param), param).Compile();
    }
}

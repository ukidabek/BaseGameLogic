using System;
using System.Linq;
using System.Reflection;

public static class AssemblyExtension
{
    /// <summary>
    /// Returns all types that extend type T.
    /// </summary>
    /// <typeparam name="T">Base type.</typeparam>
    /// <returns>List of derived types.</returns>
    public static Type[] GetDerivedTypes<T>()
    {
        return GetDerivedTypes(typeof(T));
    }

    public static Type[] GetDerivedTypes(Type baseType)
    {
        return baseType.Assembly.GetTypes().Where(type => (type.IsSubclassOf(baseType) && !type.IsAbstract)).ToArray();
    }

    public static FieldInfo[] GetAllFieldsWithAttribute<T>(this Type type)
    {
        return GetAllFieldsWithAttribute(type, typeof(T));
    }

    public static FieldInfo[] GetAllFieldsWithAttribute(Type inType, Type attributeType)
    {
        return inType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Where(
            field => field.GetCustomAttributes(false).Any(attribute => attribute.GetType() == attributeType)).ToArray();
    }
}

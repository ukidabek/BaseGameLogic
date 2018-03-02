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
        Type baseType = typeof(T);
        Assembly assembly = baseType.Assembly;
        Type[] types = assembly.GetTypes().Where(type => (type.IsSubclassOf(baseType) && !type.IsAbstract)).ToArray();

        return types;
    }
}

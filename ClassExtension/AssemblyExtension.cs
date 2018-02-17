using System;
using System.Linq;
using System.Reflection;

public static class AssemblyExtension
{
    public static Type[] GetDerivedTypes<T>()
    {
        Type baseType = typeof(T);
        Assembly assembly = baseType.Assembly;
        Type[] types = assembly.GetTypes().Where(type => (type.IsSubclassOf(baseType) && !type.IsAbstract)).ToArray();

        return types;
    }
}

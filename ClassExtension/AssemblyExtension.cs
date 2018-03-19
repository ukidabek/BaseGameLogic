﻿using System;
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
}

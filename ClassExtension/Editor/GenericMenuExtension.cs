using UnityEngine;
using UnityEditor;

using System;

public static class GenericMenuExtension 
{
    public static GenericMenu GenerateMenuFormTypes(Type[] types, GenericMenu.MenuFunction2 function)
    {
        GenericMenu menu = new GenericMenu();

        GUIContent content = null;
        for (int i = 0; i < types.Length; i++)
        {
            content = new GUIContent(types[i].Name);
            menu.AddItem(content, false, function, i);
        }

        return menu;
    }
}

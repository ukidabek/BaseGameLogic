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

    public static GenericMenu GenerateMenuFromTypesToObject(GameObject[] gameObjects, Type[] types, GenericMenu.MenuFunction2 function, bool on = false)
    {
        GenericMenu menu = new GenericMenu();
        GUIContent content = null;
        for (int i = 0; i <gameObjects.Length; i++)
        {
            for (int j = 0; j < types.Length; j++)
            {
                content = new GUIContent(string.Format("{0}/{1}", gameObjects[i] == null ? "New object" : gameObjects[i].name, types[j].Name));
                GameObjectModuleTypePair pair = new GameObjectModuleTypePair(gameObjects[i], types[j]);
                menu.AddItem(content, on, function, pair);
            }
        }

        return menu;
    }
}

public class GameObjectModuleTypePair
{
    public GameObject GameObject = null;
    public Type Type = null;

    public GameObjectModuleTypePair(GameObject gameObject, Type type)
    {
        GameObject = gameObject;
        Type = type;
    }
}

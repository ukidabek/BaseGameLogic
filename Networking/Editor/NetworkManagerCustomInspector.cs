using UnityEngine;
using UnityEditor;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace BaseGameLogic.Networking
{
    [CustomEditor(typeof(NetworkManager), true)]
    public class NetworkManagerCustomInspector : Editor
    {
        private NetworkManager manager = null;
        private Type[] types = null;
        private GenericMenu contextMenu = null;

        private void OnEnable()
        {
            manager = target as NetworkManager;

            Type baseType = typeof(BaseMessageHandler);
            Assembly assembly = baseType.Assembly;
            types = assembly.GetTypes().Where(type => (type.IsSubclassOf(baseType) && !type.IsAbstract)).ToArray();

            contextMenu = new GenericMenu();

            GUIContent content = null;
            for (int i = 0; i < types.Length; i++)
            {
                content = new GUIContent(types[i].Name);
                contextMenu.AddItem(content, false, AddMessageHandler, i);
            }
            content = new GUIContent("Add all");
            contextMenu.AddItem(content, false, AddAllMessageHandler);

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Add message handler"))
            {
                contextMenu.ShowAsContext();
            }
        }

        public void AddMessageHandler(object data)
        {
            int index = (int)data;
            Type type = types[index];
            manager.AddNewMessageHandler(type);

            Debug.Log(string.Format("{0} added.", type.Name));
        }

        public void AddAllMessageHandler()
        {
            for (int i = 0; i < types.Length; i++)
            {
                AddMessageHandler(i);
            }
        }
    }
}
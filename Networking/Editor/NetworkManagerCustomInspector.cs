using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Networking
{
    [CustomEditor(typeof(NetworkManager), true)]
    public class NetworkManagerCustomInspector : Editor
    {
        private NetworkManager manager = null;

        private void OnEnable()
        {
            manager = target as NetworkManager;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Create game"))
            {
                manager.StartSession();
            }

            if (GUILayout.Button("Join game"))
            {
                manager.JoinSession();
            }
        }
    }
}
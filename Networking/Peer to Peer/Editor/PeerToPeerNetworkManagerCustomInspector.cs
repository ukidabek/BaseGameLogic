using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Networking.PeerToPeer
{
    [CustomEditor(typeof(PeerToPearNetworkManager), true)]
    public class PeerToPeerNetworkManagerCustomInspector : Editor
    {
        private PeerToPearNetworkManager manager = null;

        private void OnEnable()
        {
            manager = target as PeerToPearNetworkManager;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Create game"))
            {
                manager.StartSession();
                manager.Connect();
            }

            if (GUILayout.Button("Join game"))
            {
                manager.JoinSession();
            }
        }
    }
}
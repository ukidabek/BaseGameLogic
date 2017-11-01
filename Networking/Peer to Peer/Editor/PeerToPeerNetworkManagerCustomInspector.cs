using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Networking.PearToPear
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

            if(GUILayout.Button("Connect"))
            {
                manager.Connect();
            }
        }
    }
}
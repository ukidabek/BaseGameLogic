using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Networking.PearToPear
{
    [CustomEditor(typeof(PearToPearNetworkManager), true)]
    public class PearToPearNetworkManagerCustomInspector : Editor
    {
        private PearToPearNetworkManager manager = null;

        private void OnEnable()
        {
            manager = target as PearToPearNetworkManager;
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
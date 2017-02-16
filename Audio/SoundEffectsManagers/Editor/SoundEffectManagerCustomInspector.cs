using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Audio
{
    [CustomEditor(typeof(BaseSoundEffectManager), true)]
    public class SoundEffectManagerCustomInspector : Editor 
    {
        private BaseSoundEffectManager _objectWithAudio = null;
        private void OnEnable()
        {
            _objectWithAudio = target as BaseSoundEffectManager;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            bool buttonPressed = GUILayout.Button("Create sound sources.");
            if (buttonPressed)
            {
                _objectWithAudio.Initialize();
            }
        }
    }
}
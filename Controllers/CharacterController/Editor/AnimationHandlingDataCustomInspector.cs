using UnityEngine;
using UnityEditor;

using System.Collections;
namespace BaseGameLogic.Audio
{   
    [CustomEditor(typeof(AnimationHandlingData), true)]
    public class AnimationHandlingDataCustomInspector : Editor
    {
        private AnimationHandlingData animationHandlingData;

        private void OnEnable()
        {
            animationHandlingData = target as AnimationHandlingData;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Inilialize"))
            {
                animationHandlingData.Initialization();
            }
        }
    }
}

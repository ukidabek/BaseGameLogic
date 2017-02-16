using UnityEngine;
using UnityEditor;
using System.Collections;

namespace BaseGameLogic
{
    [CustomEditor(typeof(BaseCameraController), true)]
    public class BaseCameraControllerCustomInspector : Editor 
    {
        private BaseCameraController baseCameraController = null;
        private Editor cameraSettingsEditor = null;

        private bool schowCameraSettingsEditor = false;

        public void OnEnable()
        {
            baseCameraController = target as BaseCameraController;
            cameraSettingsEditor = Editor.CreateEditor(baseCameraController.Settings);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            schowCameraSettingsEditor = EditorGUILayout.Foldout(schowCameraSettingsEditor, "Camera settings");
            if (schowCameraSettingsEditor && cameraSettingsEditor != null)
            {
                this.cameraSettingsEditor.OnInspectorGUI();
            }

        }
    }
}

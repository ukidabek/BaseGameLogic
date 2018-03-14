using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.LogicModule;

namespace BaseGameLogic.States
{
    [CustomEditor(typeof(BaseStateObject), true)]
    public class BaseStateObjectCustomInspector : Editor
    {

        protected virtual void OnEnable()
        {
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

        }
    }
}
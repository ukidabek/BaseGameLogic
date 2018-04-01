using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.LogicModule;

namespace BaseGameLogic.States
{
    [CustomEditor(typeof(BaseStateHandler), true)]
    public class BaseStateObjectCustomInspector : Editor
    {
        private BaseStateHandler _baseStateObject = null;
        protected virtual void OnEnable()
        {
            _baseStateObject = target as BaseStateHandler;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
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
        private BaseStateObject _baseStateObject = null;
        protected virtual void OnEnable()
        {
            _baseStateObject = target as BaseStateObject;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(_baseStateObject.LogicModulesHandler == null)
            {
                if(GUILayout.Button("Add logic module"))
                {
                    _baseStateObject.GetLogicModuleHandler();
                    if(_baseStateObject.LogicModulesHandler == null)
                        _baseStateObject.AddLogicModuleHandler();
                }
            }
            else
            {
                if (GUILayout.Button("Remove logic module"))
                    _baseStateObject.RemoveLogicModuleHandler();

            }
        }
    }
}
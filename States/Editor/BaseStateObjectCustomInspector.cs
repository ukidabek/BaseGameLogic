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
        BaseStateObject stateObject = null;

        protected virtual void OnEnable()
        {
            stateObject = target as BaseStateObject;    
        }

        private void FindAllLogicModule(GameObject gameObject)
        {
            BaseLogicModule[] baseLogicModules = gameObject.GetComponents<BaseLogicModule>();

            for (int i = 0; i < baseLogicModules.Length; i++)
            {
                try
                {
                    stateObject.LogicModulesContainer.AddModule(baseLogicModules[i]);
                }
                catch(LogicModuleOnListException e)
                {
                    Debug.Log(e.Message);
                    DestroyImmediate(e.AdditionalModule);
                }
            }

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                FindAllLogicModule(gameObject.transform.GetChild(i).gameObject);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            bool guiEnabled = GUI.enabled;
            GUI.enabled = !Application.isPlaying;

            if (GUILayout.Button("Get all logic modules"))
            {
                FindAllLogicModule(stateObject.gameObject);
            }

            GUI.enabled = guiEnabled;


            //if (GUILayout.Button("States graph"))
            //{
            //    StatTreeNode node = new StatTreeNode();
            //    StatTree tree = (target as BaseStateObject).tree;
            //    tree.nodes.Add(node);
            //    for (int i = 0; i < 100; i++)
            //    {
            //        StatTreeNode _node = new StatTreeNode();
            //        tree.nodes.Add(_node);
            //        node._outgoingNodes.Add(tree.nodes.Count - 1);
            //        node = _node;
            //    }
            //}

            //if (GUILayout.Button("States graph"))
            //{

            //    StatTree tree = (target as BaseStateObject).tree;
            //    StatTreeNode node = tree.nodes[0];
            //    while (true)
            //    {
            //        if (node == null)
            //        {
            //            Debug.Log(":(");
            //            break;
            //        }
            //        else
            //        {
            //            if (node._outgoingNodes.Count == 0)
            //                node = null;
            //            else
            //            {
            //                node = tree.nodes[node._outgoingNodes[0]];
            //            }
            //            Debug.Log(":)");
            //        }
            //    }
            //}
        }
    }
}
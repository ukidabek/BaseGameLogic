using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.States
{
    [CustomEditor(typeof(BaseStateObject), true)]
    public class BaseStateObjectCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("States graph"))
            {
                StatTreeNode node = new StatTreeNode();
                StatTree tree = (target as BaseStateObject).tree;
                tree.nodes.Add(node);
                for (int i = 0; i < 100; i++)
                {
                    StatTreeNode _node = new StatTreeNode();
                    tree.nodes.Add(_node);
                    node._outgoingNodes.Add(tree.nodes.Count - 1);
                    node = _node;
                }
            }

            if (GUILayout.Button("States graph"))
            {

                StatTree tree = (target as BaseStateObject).tree;
                StatTreeNode node = tree.nodes[0];
                while (true)
                {
                    if (node == null)
                    {
                        Debug.Log(":(");
                        break;
                    }
                    else
                    {
                        if (node._outgoingNodes.Count == 0)
                            node = null;
                        else
                        {
                            node = tree.nodes[node._outgoingNodes[0]];
                        }
                        Debug.Log(":)");
                    }
                }
            }
        }
    }
}
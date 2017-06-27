using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	public class ChainProcessorEditor : EditorWindow
	{
		private ChainProcessorEditorModeEnum _mode = ChainProcessorEditorModeEnum.Normal;

		private Event _currentEvent = null;
		private Vector2 _currentMousePositon = Vector2.zero;

		private GenericMenu _edytorContextMenu = new GenericMenu();
		private GenericMenu _nodeContextMenu = new GenericMenu();

		private List<Order> _factoryOrdersList = new List<Order>();

		private ChainProcessor _processor = null;
		public ChainProcessor Processor 
		{
			get { return this._processor; }
			set { _processor = value; }
		}

		private BaseChainLinkFactory Factory
		{
			get { return _processor.LinksFactory; }
		}

		private List<ChainLink> Links
		{
			get { return _processor.LinkList; }
		}

		private ChainLink _linkA = null;
		private ChainLink _linkB = null;

		private void GenerateEditorContextMenu()
		{
			GameObject linkContainerObject = _processor.LinkContainerObject;

			string[] linksTypes = _processor.LinksFactory.ChainLinkTypes;
			for (int i = 0; i < linksTypes.Length; i++) 
			{
				string type = linksTypes [i];
				string guiContentText = string.Format ("Create/{0}", type);

				Order newOrder = new Order (type, linkContainerObject);
				_factoryOrdersList.Add (newOrder);
				GUIContent newGuiContent = new GUIContent (guiContentText);
				_edytorContextMenu.AddItem (newGuiContent, false, CreateNode, newOrder);
			}
		}

		private void GenerateNodeContextMenu()
		{
			string guiContentText = "Connect";
			GUIContent newGuiContent = new GUIContent (guiContentText);
			_nodeContextMenu.AddItem (newGuiContent, false, ConnectNode, null);
		}

		public void Initialize (ChainProcessor _processor)
		{
			this._processor = _processor;
			this.wantsMouseMove = true;

			GenerateEditorContextMenu ();
			GenerateNodeContextMenu ();
		}

		private void NormalModeOperations()
		{
			if (_currentEvent.type == EventType.MouseDown &&
				_currentEvent.button == 1)
			{
				_currentMousePositon = _currentEvent.mousePosition;
				_linkA = FindLink (_currentMousePositon);

				if (_linkA != null)
				{
					_nodeContextMenu.ShowAsContext ();
				}
				else 
				{
					_edytorContextMenu.ShowAsContext ();
				}

				_currentEvent.Use ();
			}
		}

		private ChainLink FindLink(Vector2 position)
		{
			bool nodeClicked = false;
			for (int i = 0; i < Links.Count; i++) 
			{
				nodeClicked = Links [i].LinkRect.Contains (position);
				if (nodeClicked) 
				{
					return Links [i];
				}
			}

			return null;
		}


		private void ConnectModeOperations()
		{
			if (_currentEvent.type == EventType.MouseMove) 
			{
				_currentMousePositon = _currentEvent.mousePosition;
				_currentEvent.Use ();
			}

			if (_currentEvent.type == EventType.MouseDown &&
			    _currentEvent.button == 0) 
			{
				_currentMousePositon = _currentEvent.mousePosition;
				_linkB = FindLink (_currentMousePositon);
				_currentEvent.Use ();

				if (_linkA != null && _linkB != null) 
				{
					_linkA.Outputs.Add (_linkB);
					_linkA = _linkB = null;

					_mode = ChainProcessorEditorModeEnum.Normal;
					EditorUtility.SetDirty (_processor.gameObject);
					return;
				}
			}

			Color oldColor = Handles.color;
			Handles.color = Color.black;
			Handles.DrawLine (_linkA.LinkRect.position, _currentMousePositon);
			Handles.color = oldColor;
		}

		private void ConnectNode(object obj)
		{
			_mode = ChainProcessorEditorModeEnum.Connect;
		}

		private void CreateNode(object obj)
		{
			if (obj is Order) 
			{
				Order order = obj as Order;
				order.Position = _currentMousePositon;

				ChainLink link = Factory.FabricateChainLink (order);
				if (link != null) 
				{
					if (link is ChainInput) 
					{
						_processor.Inputs.Add (link as ChainInput);
					}

					if (link is ChainOutput) 
					{
						_processor.Outputs.Add (link as ChainOutput);
					}
					_processor.LinkList.Add (link);
				}

			}
		}

		private void  DrawNodes()
		{
			BeginWindows();
			{
				for (int i = 0; i < Links.Count; i++) 
				{
					Links [i].LinkRect = GUI.Window (
						i,
						Links [i].LinkRect,
						Links [i].DrawNodeWindow,
						Links [i].Name);
				}
			}
			EndWindows();
		}

		private void OnGUI()
		{
			if (_processor == null)
		
				return;
			_currentEvent = Event.current;

			switch (_mode) 
			{
			case ChainProcessorEditorModeEnum.Normal:
				NormalModeOperations ();
				break;

			case ChainProcessorEditorModeEnum.Connect:
				ConnectModeOperations ();
				break;
			}

			DrawNodes ();
		}
	}
}
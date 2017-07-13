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
		private const int Left_Mouse_Button = 0;
		private const int Right_Mouse_Button = 1;

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

		#region Context menu generation 

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
			_nodeContextMenu.AddItem (newGuiContent, false, SwithToConnectMode, null);

			guiContentText = "Delete";
			newGuiContent = new GUIContent (guiContentText);
			_nodeContextMenu.AddItem (newGuiContent, false, DeleteNode, null);
		}

		public void Initialize (ChainProcessor _processor)
		{
			this._processor = _processor;
			this._processor.Reprint -= Repaint;
			this._processor.Reprint += Repaint;
			this.wantsMouseMove = true;

			GenerateEditorContextMenu ();
			GenerateNodeContextMenu ();
		}

		void OnDisable() 
		{
			this._processor.Reprint -= Repaint;
		}

		#endregion

		#region Mode Operations

		/// <summary>
		/// Resets to normal node.
		/// </summary>
		public void ResetToNormalNode()
		{
			_linkA = _linkB = null;

			_mode = ChainProcessorEditorModeEnum.Normal;
			EditorUtility.SetDirty (_processor.gameObject);
		}

		/// <summary>
		/// Normals the mode operations.
		/// </summary>
		private void NormalModeOperations()
		{
			if (_currentEvent.type == EventType.MouseDown &&
				_currentEvent.button == Right_Mouse_Button)
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

			if (_currentEvent.type == EventType.MouseDown &&
				_currentEvent.button == Left_Mouse_Button) 
			{
				_currentMousePositon = _currentEvent.mousePosition;
				_linkA = FindLink (_currentMousePositon);

				ChainLink link = null;
				int index = 0;
				FindLinkByHook (
					_currentMousePositon,
					1,
					out link,
					out index);

				if (link != null) 
				{
					_linkA = link;
					SwithToConnectMode ();
				}
			}

			if (_currentEvent.type == EventType.KeyDown &&
				_currentEvent.keyCode == KeyCode.Delete) 
			{
				if (_linkA != null)
				{
					DeleteNode ();
				}
			}
		}

		/// <summary>
		/// Swiths to connect mode.
		/// </summary>
		/// <param name="obj">Object.</param>
		private void SwithToConnectMode(object obj = null)
		{
			_mode = ChainProcessorEditorModeEnum.Connect;
		}

		/// <summary>
		/// Connects the mode operations.
		/// </summary>
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
				int index = 0;
				FindLinkByHook (
					_currentMousePositon,
					0,
					out _linkB,
					out index);

				_currentEvent.Use ();

				if (_linkA != null && _linkB != null && index > -1) 
				{
					ConnectNodes (index);
					return;
				}
			}

			if ((_currentEvent.type == EventType.MouseDown &&
				_currentEvent.button == 1) ||
				(_currentEvent.type == EventType.KeyDown && 
					_currentEvent.keyCode == KeyCode.Escape)) 
			{
				ResetToNormalNode ();
				_currentEvent.Use ();
			}

			if (_linkA != null) 
			{
				DrawLine (_linkA.OutputHook (), _currentMousePositon);
			}
		}

		#endregion

		#region Nodes managnet

		/// <summary>
		/// Finds the link by his position.
		/// </summary>
		/// <returns>The link if found null if not.</returns>
		/// <param name="position">Position to cheack.</param>
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

		/// <summary>
		/// Finds the link by hook.
		/// </summary>
		/// <param name="position">Position to cheack.</param>
		/// <param name="hookType">Hook type input (0) or output (1).</param>
		/// <param name="link">Variable for link if found.</param>
		/// <param name="index">Variable for index of that node.</param>
		private void FindLinkByHook (Vector2 position, int hookType, out ChainLink link, out int index)
		{
			link = null;
			index = -1;

			bool nodeClicked = false;
			for (int i = 0; i < Links.Count; i++) 
			{
				Rect[] rects = null;
				switch (hookType) 
				{
				case 0:
					rects = Links [i].GetInputHooksRects ();
					break;
				case 1:
					rects = Links [i].GetOutputHookRects ();
					break;
				}

				if (rects == null) 
				{
					continue;
				}

				for (int j = 0; j < rects.Length; j++) 
				{					
					nodeClicked = rects[j].Contains (position);
					if (nodeClicked) 
					{
						index = j;
						link= Links [i];
						break;
					}
				}

				if (nodeClicked) 
				{
					break;
				}
			}
		}

		/// <summary>
		/// Deletes the node.
		/// </summary>
		/// <param name="obj">Link object.</param>
		private void DeleteNode(object obj = null)
		{
			ChainLink link = _linkA;
			if (link == null)
				return;

			int index =_processor.LinkList.IndexOf (link);

			for (int i = 0; i < _processor.LinkList.Count; i++) 
			{
				if (i != index) 
				{
					ChainLink _link = _processor.LinkList [i];

					for (int j = 0; j < _link.Inputs.Length; j++) 
					{
						if(_link.Inputs[j] == link)
						{
							_link.Inputs[j] = null;
						}
					}

					for (int j = 0; j < _link.Outputs.Count; j++) 
					{
						_link.Outputs.RemoveAt (j);
					}
				}
			}

			if (index >= 0) 
			{
				_processor.LinkList.RemoveAt (index);
			}

			if (link is ChainOutput) 
			{
				ChainOutput output = link as ChainOutput;
				index = _processor.Outputs.IndexOf (output);

				if (index >= 0) 
				{
					_processor.Outputs.RemoveAt (index);
				}
			}

			if (link is ChainInput) 
			{
				ChainInput input = link as ChainInput;
				index = _processor.Inputs.IndexOf (input);

				if (index >= 0) 
				{
					_processor.Inputs.RemoveAt (index);
				}
			}

			DestroyImmediate (link);
			Repaint ();
		}

		/// <summary>
		/// Connects the nodes.
		/// </summary>
		/// <param name="obj">Node input index.</param>
		public void ConnectNodes(object obj)
		{
			int index = (int)obj;

			Type type = _linkA.OutputType;
			if (_linkB.CheackConnectingOutputType (type, index)) 
			{
				_linkA.Outputs.Add (_linkB);

				_linkB.Inputs [index] = _linkA;
			}

			ResetToNormalNode ();
		}

		/// <summary>
		/// Creates the node.
		/// </summary>
		/// <param name="obj">Order object.</param>
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

		#endregion
			
		private void DrawLine (Vector2 start, Vector2 end)
		{
			Color oldColor = Handles.color;
			Handles.color = Color.black;
			Handles.DrawLine (start, end);
			Handles.color = oldColor;
		}
			
		private void  DrawNodes()
		{
			BeginWindows();
			{
				for (int i = 0; i < Links.Count; i++) 
				{
					Links [i].DrawInputHooks ();
					Links [i].DrawOutputHook ();

					Links [i].LinkRect = GUI.Window (
						i,
						Links [i].LinkRect,
						Links [i].DrawNodeWindow,
						Links [i].Name);

					if (Links [i].Inputs != null) 
					{
						for (int j = 0; j < Links [i].Inputs.Length; j++) 
						{
							if (Links [i].Inputs [j] != null) 
							{
								Vector2 start = Links [i].GetInputHook (j);
								Vector2 end = Links [i].Inputs [j].OutputHook ();
								DrawLine (start, end);
							}
						}
					}
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
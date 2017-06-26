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
		Rect contextRect = new Rect(10, 10, 100, 100);

		GenericMenu menu = new GenericMenu();
		private List<Order> _ordersList = new List<Order>();

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

		void Callback(object obj)
		{
			if (obj is Order) 
			{
				Order order = obj as Order;
				ChainLink link = Factory.FabricateChainLink (order);
				if (link != null) 
				{
					_processor.LinkList.Add (link);
				}

			}
		}

		public void Initialize (ChainProcessor _processor)
		{
			this._processor = _processor;

			GameObject linkContainerObject = _processor.LinkContainerObject;

			string[] linksTypes = _processor.LinksFactory.ChainLinkTypes;
			for (int i = 0; i < linksTypes.Length; i++) 
			{
				string type = linksTypes [i];
				string guiContentText = string.Format ("Create/{0}", type);

				Order newOrder = new Order (type, linkContainerObject);
				_ordersList.Add (newOrder);
				GUIContent newGuiContent = new GUIContent (guiContentText);
				menu.AddItem (newGuiContent, false, Callback, newOrder);
			}

		}
		

		private void OnGUI()
		{
			if (_processor == null)
				return;
			List<ChainLink> _links = _processor.LinkList;

			BeginWindows();
			{
				for (int i = 0; i < _links.Count; i++) 
				{
					_links [i].LinkRect = GUI.Window (
						i,
						_links [i].LinkRect,
						_links [i].DrawNodeWindow,
						_links [i].Name);
				}
			}
			EndWindows();

			Event currentEvent = Event.current;
			if (currentEvent.type == EventType.ContextClick)
			{
				Vector2 mousePositon = currentEvent.mousePosition;

				for (int i = 0; i < _ordersList.Count; i++) 
				{
					_ordersList [i].Position = mousePositon;
				}
//				if (contextRect.Contains(mousePos))
//				{
					// Now create the menu, add items and show it

//				menu.AddItem(new GUIContent("MenuItem1"), false, Callback, "item 1");
//				menu.AddItem(new GUIContent("MenuItem2"), false, Callback, "item 2");
//				menu.AddSeparator("");
//				menu.AddItem(new GUIContent("SubMenu/MenuItem3"), false, Callback, "item 3");
				menu.ShowAsContext();
				currentEvent.Use();
//				}
			}
		}
	}
}
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	[Serializable]
	public abstract class ChainLink : MonoBehaviour
	{
		#if UNITY_EDITOR
		[SerializeField]
		protected Rect _linkRect = new Rect ();
		public Rect LinkRect 
		{
			get { return this._linkRect; }
			set { _linkRect = value; }
		}
		#endif

		public abstract string Name { get; }

		public virtual ChainData Data { get; set;}

		public abstract Vector2 Size { get; }

		public ChainLink (Vector2 position)
		{
			#if UNITY_EDITOR
			this._linkRect.position = position;
			this._linkRect.size = Size;
			#endif
		}

		protected abstract int InputsCount { get ; }

		[SerializeField]
		protected ChainLink[] _inputs = null; 
		public ChainLink [] Inputs 
		{
			get 
			{ 
				if (_inputs == null) 
				{
					_inputs = new ChainLink [InputsCount];
				}

				return _inputs; 
			}
		}


		[SerializeField]
		public List<ChainLink> _outputs = null;
		public List<ChainLink> Outputs 
		{ 
			get 
			{
				if (_outputs == null) 
				{
					_outputs = new List<ChainLink>();
				}
				return _outputs; 
			} 
		}

		public abstract void Prosess();

		#if UNITY_EDITOR
		public virtual Vector2 OutputHook()
		{
			Vector2 hook = new Vector2 (
				_linkRect.position.x + _linkRect.size.x,
				_linkRect.position.y + (_linkRect.size.y / 2));

			return hook;
		}

		public Rect [] GetOutputHookRects()
		{
			Rect[] rects = new Rect[1];
			Vector2 size = new Vector2 (10, 10);
			Vector2 inputHook = OutputHook ();

			rects [0] = new Rect (
				new Vector2 (
					inputHook.x - (size.x / 2),
					inputHook.y - (size.y / 2)),
				size);

			return rects;
		}

		public Vector2 GetInputHook(int index)
		{
			float deltaHeight = _linkRect.size.y / (InputsCount * 2);

			Vector2 hook = new Vector2 (
				_linkRect.position.x,
				_linkRect.position.y + (deltaHeight + (deltaHeight * index * 2)));

			return hook;
		}

		public Rect [] GetInputHooksRects()
		{
			if (InputsCount == 0)
				return null;

			Rect[] rects = new Rect[InputsCount];

			Vector2 size = new Vector2 (10, 10);

			for (int i = 0; i < InputsCount; i++)
			{
				Vector2 inputHook = GetInputHook (i);
				rects [i] = new Rect (
					new Vector2 (
						inputHook.x - (size.x / 2),
						inputHook.y - (size.y / 2)),
					size);
			}

			return rects;
		}

		public void DrawInputHooks()
		{
			Rect[] x = GetInputHooksRects ();
			if (x != null) 
			{
				for (int i = 0; i < x.Length; i++)
				{
					EditorGUI.DrawRect (x [i], Color.green);
				}
			}
		}

		public virtual void DrawOutputHook()
		{
			Rect[] x = GetOutputHookRects ();
			if (x != null) 
			{
				EditorGUI.DrawRect (x [0], Color.green);
			}
		}

		public virtual void DrawNodeWindow(int id) 
		{
			if (Data != null) 
			{
				Data.DrawDataOnInspektorGUI ();
			}

			GUI.DragWindow();
		}

		public virtual bool CheackConnectingOutputType(Type type, int inputIdex)
		{
			ChainLink link = null;
			Type _type = GetType (link);
			return type == _type;
		}

		protected Type GetType<T>(T obj) 
		{
			Type type;
			if (obj == null) 
			{
				type = typeof(T);
			} 
			else 
			{
				type = obj.GetType ();
			}

			return type;
		}

		public virtual Type OutputType
		{
			get 
			{
				ChainLink link = null;
				return GetType (link);
			}
		}
		#endif
	}
}
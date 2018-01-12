using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Events
{
	public class EventThrower : EditorWindow 
	{
		private const string Event_Thrower_Config_Patch = "Assets/Scripts/BaseGameLogic/Managers/EventManager/EventThrover/EventThrowerConfig.asset";

	    private Vector2 scrollPosition = Vector2.zero;
	    public Vector2 ScrollPosition 
		{
	        get 
			{
	            return this.scrollPosition;
	        }
	        set { scrollPosition = value; }
	    }
	    
	    private static EventThrowerConfig config = null;
	    public EventThrowerConfig Config 
		{
	        get 
			{
	            if(config == null) 
	            {
					string path = Event_Thrower_Config_Patch;
					config = (EventThrowerConfig)AssetDatabase.LoadAssetAtPath(path, typeof(EventThrowerConfig));
	            }   

	            return config;
	        }
	    }
	    
	    private static EventThrower window;
	    public EventThrower Window 
		{
	        get 
			{
	            if(window == null) 
				{
	                window = this;
	            }
	            
	            return window;
	        }
	        set { window = value; }
	    }

	    private const string WindowTitle = "Event Thrower";
	    private const string MenuLocation = "Utility/Event Thrower";
	    private const string OpenWindowLocation = "";
	    
	    private int EventsCount 
		{
	        get 
			{ 
				if(Config != null)
					return Config.EventsToThrow.Count; 

				return 0;
			}
	    }
	    
	    private List<EventToThrow> EventsList 
		{
	        get { return Config.EventsToThrow; }
	    }
	    
	    [MenuItem(MenuLocation + OpenWindowLocation)]
	    public static void Init()
	    {
	        window = EditorWindow.GetWindow<EventThrower>();
	        window.titleContent = new GUIContent(WindowTitle);
	        window.Show();        
	    }
	    
	    protected void OnGUI() 
	    {
	        Color oldColor = GUI.backgroundColor;
	        GUI.backgroundColor = new Color(
	            Random.Range(0.3f, 1f),
	            Random.Range(0.3f, 1f),
	            Random.Range(0.3f, 1f));
	        
	        if(GUILayout.Button("Select config")) 
			{
	            Selection.activeObject = Config;
	        }

	        GUI.backgroundColor = oldColor;
	        
	        ScrollPosition = EditorGUILayout.BeginScrollView(
				ScrollPosition, 
				GUILayout.Width(Window.position.width), 
				GUILayout.Height(Window.position.height));
	        {
	            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
	            {
	                for(int i = 0; i < EventsCount; i++) 
					{
						EventToThrow eventToThrow = EventsList [i];
						if (eventToThrow == null)
							continue;
						
						if(GUILayout.Button(eventToThrow.name)) 
						{
	                        EventsList[i].Throw();
	                    }           
	                }
	            }
	            EditorGUI.EndDisabledGroup();
	        }
	        EditorGUILayout.EndScrollView();
	    }
	}
}
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using ToryFramework.Scene;
using ToryFramework.SMB;

namespace ToryFramework.Editor
{
	[CustomEditor(typeof(TorySceneSMB))]
	[CanEditMultipleObjects]
	public class TorySceneSMBEditor : UnityEditor.Editor
	{
		#region FIELDS

		// Warning disable: https://answers.unity.com/questions/21796/disable-warning-messages.html
		#pragma warning disable 0414 // private field assigned but not used.
		TorySceneSMB smb;

		// Serialzied property.
		SerializedProperty first;
		SerializedProperty loadUnityScene;
		SerializedProperty unitySceneIndex;
		SerializedProperty stageCount;

		// Animator
		AnimatorState s;
		AnimatorStateMachine sm;

		AnimatorController ac;

		#endregion



		#region PROPERTIES

		#endregion



		#region EVENTS

		#endregion



		#region UNITY_FRAMEWORK

		void OnEnable()
		{
			// State machine behaviour
			smb = target as TorySceneSMB;

			// Serialzed properties
			first = serializedObject.FindProperty("first");
			loadUnityScene = serializedObject.FindProperty("loadUnityScene");
			unitySceneIndex = serializedObject.FindProperty("unitySceneIndex");
			stageCount = serializedObject.FindProperty("stageCount");

			// Access to the selected state or state machine in animator controller.
			// Ref.: https://docs.unity3d.com/ScriptReference/Selection.html
			// Ref.: https://docs.unity3d.com/ScriptReference/Animations.AnimatorController.html
			s = Selection.activeObject as AnimatorState;
			sm = Selection.activeObject as AnimatorStateMachine;

			// Find the animator controller of this state machine behavior belongs.
			// Ref.: https://forum.unity.com/threads/get-animator-in-editor-mode.461838/
			EditorWindow w = EditorWindow.focusedWindow;
			System.Type type = w.GetType();
			PropertyInfo prop = type.GetProperty("animatorController", (BindingFlags.NonPublic | 
			                                                            BindingFlags.Public | 
			                                                            BindingFlags.Instance));
			if (prop != null)
			{
				ac = prop.GetValue(w, null) as AnimatorController;
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update ();

			// Display MonoScript
			//MonoScript script = MonoScript.FromMonoBehaviour(target as MonoBehaviour);
			//EditorGUI.BeginDisabledGroup(true);
			//EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
			//EditorGUI.EndDisabledGroup();

			// Set the name of the state machine.
			if (s != null)
			{
				EditorGUILayout.HelpBox("Please attach this script to the most top layer of the animator.", MessageType.Error);
			}
			else if (sm != null)
			{
				if (ac != null)
				{
					if (ac.layers[0].stateMachine == sm)
					{
						sm.name = "SCENE";
					}
					else
					{
						EditorGUILayout.HelpBox("Please attach this script to the most top layer of the animator.", MessageType.Error);
					}
				}
			}

			// Help boxes
			EditorGUILayout.HelpBox("This animator controller does not acually work on playing. You can use this to set a basic flow of the scene and define initial values before the game starts.", MessageType.Info);

			// Connection info.
			if (sm != null)
			{
				bool md = false; // Can match the destination?
				if (ac != null)
				{
					AnimatorState ds = ac.layers[0].stateMachine.defaultState;

					// Check the name of the destination state.
					string[] en = System.Enum.GetNames(typeof(State));
					for (int i = 0; i < en.Length; i++)
					{
						if (string.Equals(en[i], ds.name))
						{
							first.enumValueIndex = (int)System.Enum.Parse(typeof(State), ds.name);
							md = true;
						}
					}
				}

				if (!md)
				{
					EditorGUILayout.HelpBox("The first state is not a valid tory scene.", MessageType.Error);
				}
			}
			EditorGUILayout.PropertyField(first, new GUIContent("First Tory Scene"));

			// Load Unity scene
			EditorGUILayout.PropertyField(loadUnityScene);

			// Unity scene build index
			if (loadUnityScene.boolValue)
			{
				EditorGUILayout.PropertyField(unitySceneIndex);
			}

			// Stage count.
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(stageCount);
			if (EditorGUI.EndChangeCheck())
			{
				stageCount.intValue = Mathf.Max(1, stageCount.intValue);
			}

			serializedObject.ApplyModifiedProperties();
		}



		#endregion



		#region CUSTOM_FRAMEWORK

		#endregion



		#region EVENT_HANDLERS

		#endregion



		#region METHODS

		#endregion
	}
}
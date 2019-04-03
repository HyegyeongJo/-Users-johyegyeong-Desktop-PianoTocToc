using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using ToryFramework.Scene;
using ToryFramework.SMB;

namespace ToryFramework.Editor
{
	public class ToryCustomSceneSMBEditor : UnityEditor.Editor
	{
		#region FIELDS

		// Warning disable: https://answers.unity.com/questions/21796/disable-warning-messages.html
		#pragma warning disable 0414 // private field assigned but not used.
		ToryCustomSceneSMB smb;

		// Serialzied property.
		SerializedProperty next;
		SerializedProperty forcedStayTimeSinceStarted;
		SerializedProperty interactionCheckTime;
		SerializedProperty autoLoadOnPlayerLeft;
		SerializedProperty transitionTime;
		SerializedProperty autoProceedOnTransition;
		SerializedProperty stepCount;

		// Animator
		AnimatorState s;
		AnimatorStateMachine sm;

		//AnimatorController ac;
		//AnimatorStateMachine psm;	// Parent state machine

		#endregion



		#region PROPERTIES

		protected virtual string Name 			{ get { return ""; }}

		#endregion



		#region EVENTS

		#endregion



		#region UNITY_FRAMEWORK

		protected void OnEnable()
		{
			// State machine behaviour
			smb = target as ToryCustomSceneSMB;

			// Serialzed properties
			next = serializedObject.FindProperty("next");
			forcedStayTimeSinceStarted = serializedObject.FindProperty("forcedStayTimeSinceStarted");
			interactionCheckTime = serializedObject.FindProperty("interactionCheckTime");
			autoLoadOnPlayerLeft = serializedObject.FindProperty("autoLoadOnPlayerLeft");
			transitionTime = serializedObject.FindProperty("transitionTime");
			autoProceedOnTransition = serializedObject.FindProperty("autoProceedOnTransition");
			stepCount = serializedObject.FindProperty("stepCount");

			// Access to the selected state or state machine in animator controller.
			// Ref.: https://docs.unity3d.com/ScriptReference/Selection.html
			// Ref.: https://docs.unity3d.com/ScriptReference/Animations.AnimatorController.html
			s = Selection.activeObject as AnimatorState;
			sm = Selection.activeObject as AnimatorStateMachine;

			// Find the animator controller of this state machine behavior belongs.
			// Ref.: https://forum.unity.com/threads/get-animator-in-editor-mode.461838/
			//EditorWindow w = EditorWindow.focusedWindow;
			//System.Type type = w.GetType();
			//PropertyInfo prop = type.GetProperty("animatorController", (BindingFlags.NonPublic | 
			//                                                            BindingFlags.Public | 
			//                                                            BindingFlags.Instance));
			//if (prop != null)
			//{
			//	ac = prop.GetValue(w, null) as AnimatorController;
			//}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update ();

			// DisStage MonoScript
			//MonoScript script = MonoScript.FromMonoBehaviour(target as MonoBehaviour);
			//EditorGUI.BeginDisabledGroup(true);
			//EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
			//EditorGUI.EndDisabledGroup();

			// Set the name of the state machine.
			if (s != null)
			{
				s.name = Name;
			}
			else if (sm != null)
			{
				// Help boxes
				EditorGUILayout.HelpBox("Please set this tory scene to the state, not to the state machine.", MessageType.Error);
			}

			// Connection info.
			if (s != null)
			{
				// Next ToryScene
				if (s.transitions.Length == 0)
				{
					EditorGUILayout.HelpBox("Please make transition.", MessageType.Error);
					next.enumValueIndex = (int)State.NULL;
				}
				else
				{
					int c = 0, ii = 0;
					for (int i = 0; i < s.transitions.Length; i++)
					{
						// Set transition
						s.transitions[i].hasExitTime = false;
						s.transitions[i].duration = 0f;

						// Check the next
						for (int j = 0; j < s.transitions[i].conditions.Length; j++)
						{
							if (string.Equals(s.transitions[i].conditions[j].parameter, "NEXT"))
							{
								s.transitions[i].name = "NEXT";
								c++;
								ii = i;
							}
						}
					}

					// Set the next
					if (c == 0)
					{
						EditorGUILayout.HelpBox("Please set the condition of the transition to NEXT.", MessageType.Error);
					}
					else if (c == 1)
					{
						AnimatorState ds = s.transitions[ii].destinationState;
						AnimatorStateMachine dsm = s.transitions[ii].destinationStateMachine;
						bool e = s.transitions[ii].isExit;

						bool md = false; // Can match the destination?
						if (e)
						{
							next.enumValueIndex = (int)System.Enum.Parse(typeof(State), "EXIT");
						}
						else if (ds != null)
						{
							// Check the name of the destination state.
							string[] en = System.Enum.GetNames(typeof(State));
							for (int i = 0; i < en.Length; i++)
							{
								if (string.Equals(en[i], ds.name))
								{
									next.enumValueIndex = (int)System.Enum.Parse(typeof(State), ds.name);
									md = true;
								}
							}
						}
						else if (dsm != null)
						{
							// Check the name of the destination state machine.
							string[] en = System.Enum.GetNames(typeof(State));
							for (int i = 0; i < en.Length; i++)
							{
								if (string.Equals(en[i], dsm.name))
								{
									next.enumValueIndex = (int)System.Enum.Parse(typeof(State), dsm.name);
									md = true;
								}
							}
						}

						if (!md && !e)
						{
							EditorGUILayout.HelpBox("The next state is not a valid tory scene.", MessageType.Error);
						}
					}
					else
					{
						EditorGUILayout.HelpBox("There must be only one NEXT condition of the transition.", MessageType.Error);
						next.enumValueIndex = (int)State.NULL;
					}
				}
			}
			EditorGUILayout.PropertyField(next, new GUIContent("Next Tory Scene"));

			// Others

			// Force stay time.
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(forcedStayTimeSinceStarted);
			if (EditorGUI.EndChangeCheck())
			{
				forcedStayTimeSinceStarted.floatValue = Mathf.Max(0f, forcedStayTimeSinceStarted.floatValue);
			}

			// Interaction check time.
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(interactionCheckTime);
			if (EditorGUI.EndChangeCheck())
			{
				interactionCheckTime.floatValue = Mathf.Max(0f, interactionCheckTime.floatValue);
			}

			// Auto load on player left
			if (interactionCheckTime.floatValue > 0f)
			{
				//EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(autoLoadOnPlayerLeft);
				//EditorGUI.indentLevel--;
			}

			// Transition time.
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(transitionTime);
			if (EditorGUI.EndChangeCheck())
			{
				transitionTime.floatValue = Mathf.Max(0f, transitionTime.floatValue);
			}

			// Auto proceed on transition.
			if (transitionTime.floatValue > 0f)
			{
				//EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(autoProceedOnTransition);
				//EditorGUI.indentLevel--;
			}

			// Step count.
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(stepCount);
			if (EditorGUI.EndChangeCheck())
			{
				stepCount.intValue = Mathf.Max(1, stepCount.intValue);
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


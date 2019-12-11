using UnityEngine;
using UnityEditor;
using ToryFramework.Behaviour;

namespace ToryFramework.Editor
{
	[CustomEditor(typeof(ToryBehaviour), true)]
	[CanEditMultipleObjects]
	public class ToryBehaviourEditor : UnityEditor.Editor
	{
		//#region FIELDS

		//// ToryBehaviour
		////ToryBehaviour behaviour;

		//// Values
		//SerializedProperty useTitleStarted;
		//SerializedProperty useTitleUpdated;
		//SerializedProperty useTitleFixedUpdated;
		//SerializedProperty useTitleLateUpdated;
		//SerializedProperty useTitleEnded;

		//SerializedProperty useGuideStarted;
		//SerializedProperty useGuideUpdated;
		//SerializedProperty useGuideFixedUpdated;
		//SerializedProperty useGuideLateUpdated;
		//SerializedProperty useGuideEnded;

		//SerializedProperty useStageStarted;
		//SerializedProperty useStageUpdated;
		//SerializedProperty useStageFixedUpdated;
		//SerializedProperty useStageLateUpdated;
		//SerializedProperty useStageEnded;

		//SerializedProperty useResultStarted;
		//SerializedProperty useResultUpdated;
		//SerializedProperty useResultFixedUpdated;
		//SerializedProperty useResultLateUpdated;
		//SerializedProperty useResultEnded;

		//SerializedProperty useTransitionStarted;
		//SerializedProperty useTransitionUpdated;
		//SerializedProperty useTransitionFixedUpdated;
		//SerializedProperty useTransitionLateUpdated;
		//SerializedProperty useTransitionEnded;


		//#endregion



		//#region PROPERTIES

		//#endregion



		//#region EVENTS

		//#endregion



		//#region UNITY_FRAMEWORK

		//void OnEnable()
		//{
		//	//behaviour = target as ToryBehaviour;

		//	// Values
		//	useTitleStarted = serializedObject.FindProperty("useTitleStarted");
		//	useTitleUpdated = serializedObject.FindProperty("useTitleUpdated");
		//	useTitleFixedUpdated = serializedObject.FindProperty("useTitleFixedUpdated");
		//	useTitleLateUpdated = serializedObject.FindProperty("useTitleLateUpdated");
		//	useTitleEnded = serializedObject.FindProperty("useTitleEnded");

		//	useGuideStarted = serializedObject.FindProperty("useGuideStarted");
		//	useGuideUpdated = serializedObject.FindProperty("useGuideUpdated");
		//	useGuideFixedUpdated = serializedObject.FindProperty("useGuideFixedUpdated");
		//	useGuideLateUpdated = serializedObject.FindProperty("useGuideLateUpdated");
		//	useGuideEnded = serializedObject.FindProperty("useGuideEnded");

		//	useStageStarted = serializedObject.FindProperty("useStageStarted");
		//	useStageUpdated = serializedObject.FindProperty("useStageUpdated");
		//	useStageFixedUpdated = serializedObject.FindProperty("useStageFixedUpdated");
		//	useStageLateUpdated = serializedObject.FindProperty("useStageLateUpdated");
		//	useStageEnded = serializedObject.FindProperty("useStageEnded");

		//	useResultStarted = serializedObject.FindProperty("useResultStarted");
		//	useResultUpdated = serializedObject.FindProperty("useResultUpdated");
		//	useResultFixedUpdated = serializedObject.FindProperty("useResultFixedUpdated");
		//	useResultLateUpdated = serializedObject.FindProperty("useResultLateUpdated");
		//	useResultEnded = serializedObject.FindProperty("useResultEnded");

		//	useTransitionStarted = serializedObject.FindProperty("useTransitionStarted");
		//	useTransitionUpdated = serializedObject.FindProperty("useTransitionUpdated");
		//	useTransitionFixedUpdated = serializedObject.FindProperty("useTransitionFixedUpdated");
		//	useTransitionLateUpdated = serializedObject.FindProperty("useTransitionLateUpdated");
		//	useTransitionEnded = serializedObject.FindProperty("useTransitionEnded");
		//}

		//public override void OnInspectorGUI()
		//{
		//	serializedObject.Update();
		//	{
		//		// Display MonoScript
		//		//MonoScript script = MonoScript.FromMonoBehaviour(target as MonoBehaviour);
		//		//EditorGUI.BeginDisabledGroup(true);
		//		//EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
		//		//EditorGUI.EndDisabledGroup();

		//		//EditorGUILayout.PropertyField(useTitleStarted);

		//		EditorGUILayout.BeginVertical();
		//		{
		//			EditorGUILayout.BeginHorizontal();
		//			{
		//				EditorGUILayout.PrefixLabel(" ");

		//				GUIStyle style = GUI.skin.GetStyle("Label");
		//				style.alignment = TextAnchor.MiddleLeft;
		//				EditorGUILayout.SelectableLabel("Start", style);
		//				EditorGUILayout.SelectableLabel("Update", style);
		//				EditorGUILayout.SelectableLabel("Late\nUpdate", style);
		//				EditorGUILayout.SelectableLabel("Fixed\nUpdate", style);
		//				EditorGUILayout.SelectableLabel("End", style);
		//			}
		//			EditorGUILayout.EndHorizontal();

		//			EditorGUILayout.BeginHorizontal();
		//			{
		//				EditorGUILayout.PrefixLabel("Title");
		//				useTitleStarted.boolValue = EditorGUILayout.Toggle(useTitleStarted.boolValue);
		//				useTitleUpdated.boolValue = EditorGUILayout.Toggle(useTitleUpdated.boolValue);
		//				useTitleLateUpdated.boolValue = EditorGUILayout.Toggle(useTitleLateUpdated.boolValue);
		//				useTitleFixedUpdated.boolValue = EditorGUILayout.Toggle(useTitleFixedUpdated.boolValue);
		//				useTitleEnded.boolValue = EditorGUILayout.Toggle(useTitleEnded.boolValue);
		//			}
		//			EditorGUILayout.EndHorizontal();

		//			EditorGUILayout.BeginHorizontal();
		//			{
		//				EditorGUILayout.PrefixLabel("Guide");
		//				useGuideStarted.boolValue = EditorGUILayout.Toggle(useGuideStarted.boolValue);
		//				useGuideUpdated.boolValue = EditorGUILayout.Toggle(useGuideUpdated.boolValue);
		//				useGuideLateUpdated.boolValue = EditorGUILayout.Toggle(useGuideLateUpdated.boolValue);
		//				useGuideFixedUpdated.boolValue = EditorGUILayout.Toggle(useGuideFixedUpdated.boolValue);
		//				useGuideEnded.boolValue = EditorGUILayout.Toggle(useGuideEnded.boolValue);
		//			}
		//			EditorGUILayout.EndHorizontal();

		//			EditorGUILayout.BeginHorizontal();
		//			{
		//				EditorGUILayout.PrefixLabel("Stage");
		//				useStageStarted.boolValue = EditorGUILayout.Toggle(useStageStarted.boolValue);
		//				useStageUpdated.boolValue = EditorGUILayout.Toggle(useStageUpdated.boolValue);
		//				useStageLateUpdated.boolValue = EditorGUILayout.Toggle(useStageLateUpdated.boolValue);
		//				useStageFixedUpdated.boolValue = EditorGUILayout.Toggle(useStageFixedUpdated.boolValue);
		//				useStageEnded.boolValue = EditorGUILayout.Toggle(useStageEnded.boolValue);
		//			}
		//			EditorGUILayout.EndHorizontal();

		//			EditorGUILayout.BeginHorizontal();
		//			{
		//				EditorGUILayout.PrefixLabel("Result");
		//				useResultStarted.boolValue = EditorGUILayout.Toggle(useResultStarted.boolValue);
		//				useResultUpdated.boolValue = EditorGUILayout.Toggle(useResultUpdated.boolValue);
		//				useResultLateUpdated.boolValue = EditorGUILayout.Toggle(useResultLateUpdated.boolValue);
		//				useResultFixedUpdated.boolValue = EditorGUILayout.Toggle(useResultFixedUpdated.boolValue);
		//				useResultEnded.boolValue = EditorGUILayout.Toggle(useResultEnded.boolValue);
		//			}
		//			EditorGUILayout.EndHorizontal();

		//			EditorGUILayout.BeginHorizontal();
		//			{
		//				EditorGUILayout.PrefixLabel("Transition");
		//				useTransitionStarted.boolValue = EditorGUILayout.Toggle(useTransitionStarted.boolValue);
		//				useTransitionUpdated.boolValue = EditorGUILayout.Toggle(useTransitionUpdated.boolValue);
		//				useTransitionLateUpdated.boolValue = EditorGUILayout.Toggle(useTransitionLateUpdated.boolValue);
		//				useTransitionFixedUpdated.boolValue = EditorGUILayout.Toggle(useTransitionFixedUpdated.boolValue);
		//				useTransitionEnded.boolValue = EditorGUILayout.Toggle(useTransitionEnded.boolValue);
		//			}
		//			EditorGUILayout.EndHorizontal();
		//		}
		//		EditorGUILayout.EndVertical();
		//	}
		//	serializedObject.ApplyModifiedProperties();

		//	serializedObject.Update();
		//	{
		//		// Space
		//		EditorGUILayout.Space();

		//		// Draw default indpector for derived class
		//		DrawDefaultInspector();
		//	}
		//	serializedObject.ApplyModifiedProperties();
		//}

		//#endregion



		//#region CUSTOM_FRAMEWORK

		//#endregion



		//#region EVENT_HANDLERS

		//#endregion



		//#region METHODS

		//#endregion
	}
}
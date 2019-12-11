using UnityEditor;
using UnityEditor.UI;

namespace ToryUX
{
	[CustomEditor(typeof(ToryVerticalLayoutGroup))]
	public class ToryVerticalLayoutGroupEditor : HorizontalOrVerticalLayoutGroupEditor
	{
		SerializedProperty columnPaddingOnLandscape;
		SerializedProperty columnPaddingOnPortrait;

		SerializedProperty letterboxPaddingOnLandscape;
		SerializedProperty letterboxPaddingOnPortrait;

		protected override void OnEnable()
		{
			base.OnEnable();
			columnPaddingOnLandscape = serializedObject.FindProperty("columnPaddingOnLandscape");
			columnPaddingOnPortrait = serializedObject.FindProperty("columnPaddingOnPortrait");
			letterboxPaddingOnLandscape = serializedObject.FindProperty("letterboxPaddingOnLandscape");
			letterboxPaddingOnPortrait = serializedObject.FindProperty("letterboxPaddingOnPortrait");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.LabelField("Left/Right Padding", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(columnPaddingOnLandscape);
			EditorGUILayout.PropertyField(columnPaddingOnPortrait);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Top/Bottom Padding", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(letterboxPaddingOnLandscape);
			EditorGUILayout.PropertyField(letterboxPaddingOnPortrait);
			EditorGUILayout.HelpBox("Settings above will overwrite padding setting below.", MessageType.Info);
			serializedObject.ApplyModifiedProperties();
			base.OnInspectorGUI();
		}
	}
}
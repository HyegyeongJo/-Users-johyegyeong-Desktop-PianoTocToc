using UnityEngine;
using UnityEditor;

namespace ManUtils
{
	#region READ_ONLY

	[CustomPropertyDrawer(typeof(ManReadOnly))]
	public class ManReadOnlyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			GUI.enabled = false;
			EditorGUI.PropertyField(position, property, label, true);
			GUI.enabled = true;
		}
	}

	#endregion


	#region READ_ONLY_ON_PLAYING

	[CustomPropertyDrawer(typeof(ManReadOnlyOnPlaying))]
	public class ManReadOnlyOnPlayingDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if(Application.isPlaying) GUI.enabled = false;
			EditorGUI.PropertyField(position, property, label, true);
			GUI.enabled = true;
		}
	}

	#endregion
}
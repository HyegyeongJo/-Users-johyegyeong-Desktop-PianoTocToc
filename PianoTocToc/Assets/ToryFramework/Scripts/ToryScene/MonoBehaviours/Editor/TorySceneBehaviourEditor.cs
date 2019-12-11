using UnityEditor;
using ToryFramework.Behaviour;

namespace ToryFramework.Editor
{
	[CustomEditor(typeof(TorySceneBehaviour))]
	public class TorySceneBehaviourEditor : UnityEditor.Editor
	{
		#region FIELDS

		// Warning disable: https://answers.unity.com/questions/21796/disable-warning-messages.html
		#pragma warning disable 0414 // private field assigned but not used.
		// ToryScene Behaviour
		TorySceneBehaviour behaviour;

		#endregion



		#region UNITY_FRAMEWORK

		void OnEnable()
		{
			behaviour = target as TorySceneBehaviour;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			// Display MonoScript
			//MonoScript script = MonoScript.FromMonoBehaviour(target as MonoBehaviour);
			//EditorGUI.BeginDisabledGroup(true);
			//EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
			//EditorGUI.EndDisabledGroup();

			// Help boxes
			EditorGUILayout.HelpBox("Keep the GameObject active. " +
			                        "Once inactivated, the Update-typed callback methods of the ToryScene is no longer called even if it is activated again later.", 
			                        MessageType.Info);
			
			EditorGUILayout.HelpBox("Use the Animator component to organize and manage the flow of the scene. " +
			                        "Use the Tory Scene State Generator to generate custom scenes.", 
			                        MessageType.Info);

			serializedObject.ApplyModifiedProperties();
		}

		#endregion



		#region METHODS

		#endregion
	}
}
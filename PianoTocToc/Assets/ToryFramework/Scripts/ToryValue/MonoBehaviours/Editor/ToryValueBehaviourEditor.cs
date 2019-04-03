using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ToryFramework.Behaviour;

namespace ToryFramework.Editor
{
	[CustomEditor(typeof(ToryValueBehaviour))]
	public class ToryValueBehaviourEditor : UnityEditor.Editor
	{
		#region FIELDS

		// Warning disable: https://answers.unity.com/questions/21796/disable-warning-messages.html
		#pragma warning disable 0414 // private field assigned but not used.
		// Behaviour
		ToryValueBehaviour behaviour;

		#endregion



		#region UNITY_FRAMEWORK

		void OnEnable()
		{
			// Behaviour
			behaviour = target as ToryValueBehaviour;
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
			EditorGUILayout.HelpBox("You can use this values in scripts via ToryFramework.ToryValue.Instance, or, shortly, TF.Value.",
			                        MessageType.Info);
			EditorGUILayout.HelpBox("You can define and use any type of ToryValues in your script manaully. For example, define ToryFloat variable in your script, and use it.",
			                        MessageType.Info);

			// Draw default inspector except for the script name: https://answers.unity.com/questions/316286/how-to-remove-script-field-in-inspector.html
			DrawPropertiesExcluding(serializedObject, new string[] { "m_Script" });

			serializedObject.ApplyModifiedProperties();
		}

		#endregion



		#region METHODS

		#endregion
	}
}
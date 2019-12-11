using UnityEngine;
using UnityEditor;
using ToryFramework.Behaviour;

namespace ToryFramework.Editor
{
	[CustomEditor(typeof(ToryFrameworkBehaviour))]
	public class ToryFrameworkBehaviourEditor : UnityEditor.Editor
	{
		#region FIELDS

		// Warning disable: https://answers.unity.com/questions/21796/disable-warning-messages.html
		#pragma warning disable 0414 // private field assigned but not used.
		// Behaviour
		ToryFrameworkBehaviour behaviour;

		// Editor
		SerializedProperty showLog;

		#endregion



		#region UNITY_FRAMEWORK

		void OnEnable()
		{
			// Behaviour
			behaviour = target as ToryFrameworkBehaviour;

			// Editor
			showLog = serializedObject.FindProperty("showLog");
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

			// Editor
			EditorGUILayout.PropertyField(showLog);

			serializedObject.ApplyModifiedProperties();
		}

		#endregion



		#region METHODS

		/// <summary>
		/// Sets the script execution order when Unity editor is loaded.
		/// </summary>
		/// Script execution order reference: https://github.com/kwnetzwelt/ugb-source/blob/UGB-3.0/UnityGameBase/Core/Editor/GameScriptExecutionOrder.cs
		[InitializeOnLoadMethod]
		static void SetScriptExecutionOrderOfSecureKeysManager()
		{
			// Set the target order.
			int targetOrder = -9999;

			// Get all MonoScripts.
			foreach (MonoScript monoScript in MonoImporter.GetAllRuntimeMonoScripts())
			{
				var currentClass = monoScript.GetClass();
				// Find SecureKeysManager class.
				if (currentClass == typeof(SecureKeysManager))
				{
					int currentOrder = MonoImporter.GetExecutionOrder(monoScript);

					if (currentOrder != targetOrder)
					{
						// Set the order.
						MonoImporter.SetExecutionOrder(monoScript, targetOrder);

						// Log
						Debug.Log("[ToryFramework] The script execution order of \"" + currentClass + "\" changed from " + currentOrder + " to " + targetOrder + ".");
					}
				}
			}
		}

		#endregion
	}
}
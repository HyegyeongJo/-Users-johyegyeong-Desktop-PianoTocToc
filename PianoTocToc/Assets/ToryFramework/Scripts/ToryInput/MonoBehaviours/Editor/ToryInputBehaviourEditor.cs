using System.Text;
using System.IO;
using UnityEngine;
using UnityEditor;
using ToryFramework.Input;
using ToryFramework.Behaviour;
using ToryValue;

namespace ToryFramework.Editor
{
	[CustomEditor(typeof(ToryInputBehaviour))]
	public class ToryInputBehaviourEditor : UnityEditor.Editor
	{
		#region FIELDS

		// Behaviour
		ToryInputBehaviour behaviour;

		// Data Properties
		SerializedProperty dataType;
		SerializedProperty interactionType;

		// Filters
		SerializedProperty filterType;
		SerializedProperty oefFrequency;
		SerializedProperty ensembleSize;

		// Interaction Determination
		SerializedProperty minimumInteraction;

		// Amplifier
		SerializedProperty gain;

		// Multi-input
		SerializedProperty maxMultiInputCount;

		string toryInputFileName = "ToryInput";
		string toryMultiInputFileName = "ToryMultiInput";

		#endregion



		#region UNITY_FRAMEWORK

		void OnEnable()
		{
			// Behaviour
			behaviour = target as ToryInputBehaviour;

			// Data Properties
			dataType = serializedObject.FindProperty("dataType");
			interactionType = serializedObject.FindProperty("interactionType");

			// Filters
			filterType = serializedObject.FindProperty("filterType");
			oefFrequency = serializedObject.FindProperty("oefFrequency");
			ensembleSize = serializedObject.FindProperty("ensembleSize");

			// Interaction Determination
			minimumInteraction = serializedObject.FindProperty("minimumInteraction");

			// Amplifier
			gain = serializedObject.FindProperty("gain");

			// Multi-input
			maxMultiInputCount = serializedObject.FindProperty("maxMultiInputCount");
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
			EditorGUILayout.HelpBox("Changing the Data Type will modify some ToryFramework scripts immediately, and Unity editor will reload the scripts.",
			                        MessageType.Info);

			// Data type
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(dataType);
			if (EditorGUI.EndChangeCheck())
			{
				string typeStr = "";
				switch (dataType.enumValueIndex)
				{
					case (int)DataType.Int:
						typeStr = "int";
						break;

					case (int)DataType.Float:
						typeStr = "float";
						break;

					case (int)DataType.Vector2:
						typeStr = "Vector2";
						break;

					case (int)DataType.Vector3:
						typeStr = "Vector3";
						break;

					case (int)DataType.Vector4:
						typeStr = "Vector4";
						break;

					case (int)DataType.Quaternion:
						typeStr = "Quaternion";
						break;

					case (int)DataType.KeyCode:
						typeStr = "KeyCode";
						break;

					default:
						break;
				}

				// Modify the ToryInput.cs
				// Generating a script reference: http://answers.unity3d.com/questions/1170350/editorscript-generate-enum-from-string.html
				StringBuilder code = new StringBuilder();
				code.Append("using ToryFramework.Input;\n\nnamespace ToryFramework\n{\n\tpublic class ToryInput : Tory" +
				            char.ToUpper(typeStr[0]) + typeStr.Substring(1) + 
				            "Input\n\t{\n\t\t#region SINGLETON\n\n\t\tstatic volatile ToryInput instance;\n\t\tstatic readonly object syncRoot = new object();\n\n\t\tToryInput() { }\n\n\t\tpublic static ToryInput Instance\n\t\t{\n\t\t\tget\n\t\t\t{\n\t\t\t\tif (instance == null)\n\t\t\t\t{\n\t\t\t\t\tlock (syncRoot)\n\t\t\t\t\t{\n\t\t\t\t\t\tif (instance == null)\n\t\t\t\t\t\t{\n\t\t\t\t\t\t\tinstance = new ToryInput();\n\t\t\t\t\t\t}\n\t\t\t\t\t}\n\t\t\t\t}\n\t\t\t\treturn instance;\n\t\t\t}\n\t\t}\n\n\t\t#endregion\n\t}\t\n}");
			
				// Set the path of the script.
				MonoScript script = MonoScript.FromMonoBehaviour(behaviour);
				string path = AssetDatabase.GetAssetPath(script);
				path = path.Substring(0, path.Length - Path.GetFileName(path).Length - 1);
				string[] paths = path.Split('/');
				path = "";
				for (int i = 0; i < paths.Length - 3; i++)
				{
					path += paths[i] + "/";
				}
				path += "(Do Not Update This Folder)/Scripts/ToryInput/Singletons/" + toryInputFileName + ".cs";

				// Write the script.
				if (File.Exists(path))
				{
					if (!string.Equals(code.ToString(), File.ReadAllText(path)))
					{
						File.WriteAllText(path, code.ToString());
						AssetDatabase.ImportAsset(path);
						Debug.Log("[ToryInput] The \"" + toryInputFileName + ".cs\" modified and loaded.");
					}
				}
				else
				{
					File.WriteAllText(path, code.ToString());
					AssetDatabase.ImportAsset(path);
					Debug.Log("[ToryInput] A new \"" + toryInputFileName + ".cs\" created and loaded.");
				}

				// Modifying the ToryMultiInput.cs
				// Generating a script reference: http://answers.unity3d.com/questions/1170350/editorscript-generate-enum-from-string.html
				code.Remove(0, code.Length);
				code.Append("namespace ToryFramework.Input\n{\n\tpublic class ToryMultiInput : Tory" +
				            char.ToUpper(typeStr[0]) + typeStr.Substring(1) + 
				            "MultiInput\n\t{\n\t\t#region CONSTRUCTOR\n\n\t\tpublic ToryMultiInput() : base()\n\t\t{\n\t\t}\n\n\t\tpublic ToryMultiInput(int id) : base(id)\n\t\t{\n\t\t}\n\n\t\t#endregion\n\t}\t\n}");
			
				// Set the path of the script.
				path = path.Substring(0, path.Length - Path.GetFileName(path).Length);
				path += toryMultiInputFileName + ".cs";

				// Write the script.
				if (File.Exists(path))
				{
					if (!string.Equals(code.ToString(), File.ReadAllText(path)))
					{
						File.WriteAllText(path, code.ToString());
						AssetDatabase.ImportAsset(path);
						Debug.Log("[ToryInput] The \"" + toryMultiInputFileName + ".cs\" modified and loaded.");
					}
				}
				else
				{
					File.WriteAllText(path, code.ToString());
					AssetDatabase.ImportAsset(path);
					Debug.Log("[ToryInput] A new \"" + toryMultiInputFileName + ".cs\" created and loaded.");
				}
			}

			if (dataType.enumValueIndex != (int)DataType.KeyCode)
			{
				// Filter type
				int f = filterType.FindPropertyRelative("currentValue").enumValueIndex;
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(filterType);
				if (EditorGUI.EndChangeCheck())
				{
					// Do nth.
				}

				// OEF frequency
				if (f == (int)FilterType.ONE_EURO_FILTER)
				{
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(oefFrequency, new GUIContent("OEF Frequency"));
					if (EditorGUI.EndChangeCheck())
					{
						// Restrict the value.
						oefFrequency.FindPropertyRelative("currentValue").floatValue = Mathf.Max(oefFrequency.FindPropertyRelative("currentValue").floatValue, 0f);
						oefFrequency.FindPropertyRelative("defaultValue").floatValue = Mathf.Max(oefFrequency.FindPropertyRelative("defaultValue").floatValue, 0f);
						if (SecureKeysChecker.CheckSecureKeys() &&
						    PlayerPrefsElite.key != null &&
						    PlayerPrefs.HasKey(KeyFormatter.GetSavedKey(oefFrequency.FindPropertyRelative("key").stringValue)))
						{
								float sv = PlayerPrefsElite.GetFloat(KeyFormatter.GetSavedKey(oefFrequency.FindPropertyRelative("key").stringValue));
								sv = Mathf.Max(0f, sv);
								PlayerPrefsElite.SetFloat(KeyFormatter.GetSavedKey(oefFrequency.FindPropertyRelative("key").stringValue), 
								                          sv);
						}
					}
				}
				// Ensemble average
				else if (f == (int)FilterType.ENSEMBLE_AVERAGE)
				{
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(ensembleSize);
					if (EditorGUI.EndChangeCheck())
					{
						// Restrict the value.
						ensembleSize.FindPropertyRelative("currentValue").intValue = Mathf.Max(ensembleSize.FindPropertyRelative("currentValue").intValue, 1);
						ensembleSize.FindPropertyRelative("defaultValue").intValue = Mathf.Max(ensembleSize.FindPropertyRelative("defaultValue").intValue, 1);
						if (SecureKeysChecker.CheckSecureKeys() &&
						    PlayerPrefsElite.key != null &&
						    PlayerPrefs.HasKey(KeyFormatter.GetSavedKey(ensembleSize.FindPropertyRelative("key").stringValue)))
						{
							int sv = PlayerPrefsElite.GetInt(KeyFormatter.GetSavedKey(ensembleSize.FindPropertyRelative("key").stringValue));
							sv = Mathf.Max(1, sv);
							PlayerPrefsElite.SetInt(KeyFormatter.GetSavedKey(ensembleSize.FindPropertyRelative("key").stringValue), 
							                        sv);
						}
					}
				}

				// Gain
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(gain);
				if (EditorGUI.EndChangeCheck())
				{
					// Do nth.
				}
			}

			// Interaction type
			int c = interactionType.FindPropertyRelative("currentValue").enumValueIndex;
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(interactionType);
			if (EditorGUI.EndChangeCheck())
			{
				// Do nth.
			}

			// Minimum interaction.
			if (c == (int)InteractionType.CONTINUOUS)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(minimumInteraction);
				if (EditorGUI.EndChangeCheck())
				{
					// Restrict the value.
					minimumInteraction.FindPropertyRelative("currentValue").floatValue = Mathf.Max(minimumInteraction.FindPropertyRelative("currentValue").floatValue, 0f);
					minimumInteraction.FindPropertyRelative("defaultValue").floatValue = Mathf.Max(minimumInteraction.FindPropertyRelative("defaultValue").floatValue, 0f);
					if (SecureKeysChecker.CheckSecureKeys() &&
					    PlayerPrefsElite.key != null &&
					    PlayerPrefs.HasKey(KeyFormatter.GetSavedKey(minimumInteraction.FindPropertyRelative("key").stringValue)))
					{
						float sv = PlayerPrefsElite.GetFloat(KeyFormatter.GetSavedKey(minimumInteraction.FindPropertyRelative("key").stringValue));
						sv = Mathf.Max(0f, sv);
						PlayerPrefsElite.SetFloat(KeyFormatter.GetSavedKey(minimumInteraction.FindPropertyRelative("key").stringValue), 
						                          sv);
					}
				}
			}

			// Max multi-input count
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(maxMultiInputCount);
			if (EditorGUI.EndChangeCheck())
			{
				// Restrict the value.
				maxMultiInputCount.FindPropertyRelative("currentValue").intValue = Mathf.Max(maxMultiInputCount.FindPropertyRelative("currentValue").intValue, 0);
				maxMultiInputCount.FindPropertyRelative("defaultValue").intValue = Mathf.Max(maxMultiInputCount.FindPropertyRelative("defaultValue").intValue, 0);
				if (SecureKeysChecker.CheckSecureKeys() &&
				    PlayerPrefsElite.key != null &&
				    PlayerPrefs.HasKey(KeyFormatter.GetSavedKey(maxMultiInputCount.FindPropertyRelative("key").stringValue)))
				{
					int sv = PlayerPrefsElite.GetInt(KeyFormatter.GetSavedKey(maxMultiInputCount.FindPropertyRelative("key").stringValue));
					sv = Mathf.Max(0, sv);
					PlayerPrefsElite.SetInt(KeyFormatter.GetSavedKey(maxMultiInputCount.FindPropertyRelative("key").stringValue), 
					                        sv);
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

		#endregion



		#region METHODS

		#endregion
	}
}
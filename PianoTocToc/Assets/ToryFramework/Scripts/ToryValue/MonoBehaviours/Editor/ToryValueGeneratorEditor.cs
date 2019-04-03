using System.Text;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using ToryFramework.Behaviour;
using ToryFramework.Value;

namespace ToryFramework.Editor
{
	[CustomEditor(typeof(ToryValueGenerator))]
	public class ToryValueGeneratorEditor : UnityEditor.Editor
	{
		#region FIELDS

		// Behaviour

		ToryValueGenerator behaviour;

		// Reorderable List

		ReorderableList list;

		#endregion



		#region UNITY_FRAMEWORK

		void OnEnable()
		{
			behaviour = target as ToryValueGenerator;

			// ReorderableList reference: http://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/
			// ReorderableList with struct: http://unityindepth.tistory.com/56
			list = new ReorderableList(serializedObject, serializedObject.FindProperty("values"), true, true, true, true);
			list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				var element = list.serializedProperty.GetArrayElementAtIndex(index);
				element.FindPropertyRelative("valueString").stringValue = CheckPropertyName(element.FindPropertyRelative("valueString").stringValue);
				rect.y += 2f;
				EditorGUI.PropertyField(
					new Rect(rect.x, rect.y, 70f, EditorGUIUtility.singleLineHeight),
					element.FindPropertyRelative("valueType"),
					GUIContent.none);
				EditorGUI.PropertyField(
					new Rect(rect.x + 73f, rect.y, rect.width - 73f, EditorGUIUtility.singleLineHeight),
					element.FindPropertyRelative("valueString"),
					GUIContent.none);
			};
			list.drawHeaderCallback = (Rect rect) =>
			{
				EditorGUI.LabelField(rect, "Custom Tory Values");
			};
			list.onAddCallback = (ReorderableList list) => 
			{
				var index = list.serializedProperty.arraySize;
				list.serializedProperty.arraySize += 1;
				list.index = index;
				var element = list.serializedProperty.GetArrayElementAtIndex(index);
				element.FindPropertyRelative("valueType").enumValueIndex = 0;
				element.FindPropertyRelative("valueString").stringValue = "";
			};
			list.onAddDropdownCallback = (Rect buttonRect, ReorderableList list) => {  
				var menu = new GenericMenu();

				menu.AddItem(new GUIContent("Int"), false, DropdownClickHandler, new CustomValue(ValueType.Int, ""));
				menu.AddItem(new GUIContent("Float"), false, DropdownClickHandler, new CustomValue(ValueType.Float, ""));
				menu.AddItem(new GUIContent("Bool"), false, DropdownClickHandler, new CustomValue(ValueType.Bool, ""));
				menu.AddItem(new GUIContent("String"), false, DropdownClickHandler, new CustomValue(ValueType.String, ""));
				menu.AddItem(new GUIContent("Vector2"), false, DropdownClickHandler, new CustomValue(ValueType.Vector2, ""));
				menu.AddItem(new GUIContent("Vector3"), false, DropdownClickHandler, new CustomValue(ValueType.Vector3, ""));
				menu.AddItem(new GUIContent("Vector4"), false, DropdownClickHandler, new CustomValue(ValueType.Vector4, ""));
				menu.ShowAsContext();
			};
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
			EditorGUILayout.HelpBox("Don't forget to Apply the changes. This action will modify some ToryFramework scripts immediately.", 
			                        MessageType.Info);

			EditorGUILayout.Space();

			// List
			list.DoLayoutList();

			// Button
			if (GUILayout.Button("Apply"))
			{
				// Check the array whether or not it is empty.
				int o = 0;
				for (int i = 0; i < behaviour.values.Length; i++)
				{
					if (string.IsNullOrEmpty(behaviour.values[i].valueString))
					{
						// Delete the empty element.
						list.serializedProperty.DeleteArrayElementAtIndex(i - o++);
					}
				}
				serializedObject.ApplyModifiedProperties();

				// 1. ToryValueBehaviourPartialCustom.cs
				{
					// Modify the ToryValueBehaviourPartialCustom.cs
					// Generating file reference: http://answers.unity3d.com/questions/1170350/editorscript-generate-enum-from-string.html
					StringBuilder code = new StringBuilder();
					code.Append("using UnityEngine;\nusing ToryValue;\n\nnamespace ToryFramework.Behaviour\n{\n\tpublic partial class ToryValueBehaviour : MonoBehaviour\n\t{");
					for (int i = 0; i < behaviour.values.Length; i++)
					{
						code.AppendFormat("\n\t\t[SerializeField] public Tory{0} {1};", behaviour.values[i].valueType, behaviour.values[i].valueString);
					}
					code.Append("\n\t}\n}");

					// Set the path of the ToryValueBehaviourPartialCustom.cs
					MonoScript script = MonoScript.FromMonoBehaviour(behaviour);
					string path = AssetDatabase.GetAssetPath(script);
					path = path.Substring(0, path.Length - Path.GetFileName(path).Length - 1);
					string[] paths = path.Split('/');
					path = "";
					for (int i = 0; i < paths.Length - 3; i++)
					{
						path += paths[i] + "/";
					}
					path += "(Do Not Update This Folder)/Scripts/ToryValue/MonoBehaviours/ToryValueBehaviourPartialCustom.cs";

					// Write the script to the State.cs
					if (File.Exists(path))
					{
						if (!string.Equals(code.ToString(), File.ReadAllText(path)))
						{
							File.WriteAllText(path, code.ToString());
							AssetDatabase.ImportAsset(path);
							Debug.Log("[ToryValue] The \"ToryValueBehaviourPartialCustom.cs\" modified and loaded.");
						}
					}
					else
					{
						File.WriteAllText(path, code.ToString());
						AssetDatabase.ImportAsset(path);
						Debug.Log("[ToryValue] A new \"ToryValueBehaviourPartialCustom.cs\" created and loaded.");
					}
				}

				// 2. ToryValuePartialCustom.cs
				{
					// Modify the ToryValuePartialCustom.cs
					// Generating file reference: http://answers.unity3d.com/questions/1170350/editorscript-generate-enum-from-string.html
					StringBuilder code = new StringBuilder();
					code.Append("using ToryValue;\n\nnamespace ToryFramework\n{\n\tpublic partial class ToryValue\n\t{");
					for (int i = 0; i < behaviour.values.Length; i++)
					{
						code.Append("\n\t\tpublic Tory" +
						            behaviour.values[i].valueType + 
						            " " +
						            behaviour.values[i].valueString +
						            " { get { return ValueBehaviour." +
						            behaviour.values[i].valueString +
						            "; } set { ValueBehaviour." +
						            behaviour.values[i].valueString +
						            " = value; }}");
					}
					code.Append("\n\t}\n}");

					// Set the path of the State.cs
					MonoScript script = MonoScript.FromMonoBehaviour(behaviour);
					string path = AssetDatabase.GetAssetPath(script);
					path = path.Substring(0, path.Length - Path.GetFileName(path).Length - 1);
					string[] paths = path.Split('/');
					path = "";
					for (int i = 0; i < paths.Length - 3; i++)
					{
						path += paths[i] + "/";
					}
					path += "(Do Not Update This Folder)/Scripts/ToryValue/Singletons/ToryValuePartialCustom.cs";

					// Write the script to the State.cs
					if (File.Exists(path))
					{
						if (!string.Equals(code.ToString(), File.ReadAllText(path)))
						{
							File.WriteAllText(path, code.ToString());
							AssetDatabase.ImportAsset(path);
							Debug.Log("[ToryValue] The \"ToryValuePartialCustom.cs\" modified and loaded.");
						}
					}
					else
					{
						File.WriteAllText(path, code.ToString());
						AssetDatabase.ImportAsset(path);
						Debug.Log("[ToryValue] A new \"ToryValuePartialCustom.cs\" created and loaded.");
					}
				}

				// Reload the assets.
				AssetDatabase.Refresh();
			}

			serializedObject.ApplyModifiedProperties();
		}

		#endregion



		#region METHODS

		// Check the special characters reference: https://stackoverflow.com/questions/1120198/most-efficient-way-to-remove-special-characters-from-string
		public string CheckPropertyName(string str)
		{
			// Empty check
			if (string.IsNullOrEmpty(str))
			{
				return string.Empty;
			}

			// String builder.
			StringBuilder sb = new StringBuilder();

			// Check the first letter.
			char c0 = str[0];
			if ((c0 >= 'A' && c0 <= 'Z') || 
			    (c0 == '_'))				
			{
				sb.Append(c0);
			}
			else if (c0 >= 'a' && c0 <= 'z')
			{
				sb.Append(char.ToUpper(c0));
			}

			// Check the remaining letters.
			str = str.Substring(1);
			foreach (char c in str) {
				if ((c >= '0' && c <= '9') || 
				    (c >= 'A' && c <= 'Z') || 
				    (c >= 'a' && c <= 'z') || 
				    c == '_') {
					sb.Append(c);
				}
			}
			return sb.ToString();
		}

		void DropdownClickHandler(object userData)
		{
			CustomValue data = (CustomValue)userData;
			int index = list.serializedProperty.arraySize;
			list.serializedProperty.arraySize++;
			list.index = index;
			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			element.FindPropertyRelative("valueType").enumValueIndex = (int)data.valueType;
			element.FindPropertyRelative("valueString").stringValue = "";
			serializedObject.ApplyModifiedProperties();
		}

		#endregion
	}
}
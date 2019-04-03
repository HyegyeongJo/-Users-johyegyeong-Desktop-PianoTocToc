using System.Text;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using ToryFramework.Behaviour;

namespace ToryFramework.Editor
{
	[CustomEditor(typeof(TorySceneStateGenerator))]
	public class TorySceneStateGeneratorEditor : UnityEditor.Editor
	{
		#region FIELDS

		// Behaviour

		TorySceneStateGenerator behaviour;

		// Reorderable List

		ReorderableList list;

		#endregion



		#region UNITY_FRAMEWORK

		void OnEnable()
		{
			behaviour = target as TorySceneStateGenerator;

			// ReorderbleList reference: http://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/
			list = new ReorderableList(serializedObject, serializedObject.FindProperty("states"), true, true, true, true);
			list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				var element = list.serializedProperty.GetArrayElementAtIndex(index);
				element.stringValue = CheckClassName(element.stringValue);
				rect.y += 2f;
				EditorGUI.PropertyField(
					new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
					element,
					GUIContent.none);
			};
			list.drawHeaderCallback = (Rect rect) =>
			{
				EditorGUI.LabelField(rect, "Custom Tory Scene States");
			};
			list.onAddCallback = (ReorderableList list) => 
			{
				var index = list.serializedProperty.arraySize;
				list.serializedProperty.arraySize += 1;
				list.index = index;
				var element = list.serializedProperty.GetArrayElementAtIndex(index);
				element.stringValue = "";
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
			EditorGUILayout.HelpBox("Don't forget to Apply the changes. This action will create or modify some ToryFramework scripts immediately.", 
			                        MessageType.Info);
			EditorGUILayout.HelpBox("Don't forget to manage the Animator Controller after applying.", 
			                        MessageType.Info);

			EditorGUILayout.Space();

			// List
			list.DoLayoutList();

			// Button
			if (GUILayout.Button("Apply"))
			{
				// Check the array whether or not it is empty.
				int o = 0;
				for (int i = 0; i < behaviour.States.Length; i++)
				{
					if (string.IsNullOrEmpty(behaviour.States[i]))
					{
						// Delete the empty element.
						list.serializedProperty.DeleteArrayElementAtIndex(i - o++);
					}
				}
				serializedObject.ApplyModifiedProperties();

				// 1. State.cs
				{
					// Modify the State.cs
					// Generating file reference: http://answers.unity3d.com/questions/1170350/editorscript-generate-enum-from-string.html
					StringBuilder code = new StringBuilder();
					code.Append("namespace ToryFramework.Scene\n{\n\tpublic enum State { NULL, ");
					for (int i = 0; i < behaviour.States.Length; i++)
					{
						code.AppendFormat("{0}, ", behaviour.States[i].ToUpper());
					}
					code.Append("EXIT }\n}");

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
					path += "(Do Not Update This Folder)/Scripts/ToryScene/Core/State.cs";

					// Write the script to the State.cs
					if (File.Exists(path))
					{
						if (!string.Equals(code.ToString(), File.ReadAllText(path)))
						{
							File.WriteAllText(path, code.ToString());
							AssetDatabase.ImportAsset(path);
							Debug.Log("[ToryScene] The \"State.cs\" modified and loaded.");
						}
					}
					else
					{
						File.WriteAllText(path, code.ToString());
						AssetDatabase.ImportAsset(path);
						Debug.Log("[ToryScene] A new \"State.cs\" created and loaded.");
					}
				}

				// 2. TorySceneStatePartialNext.cs
				{
					// Modify the TorySceneStatePartialNext.cs
					// Generating file reference: http://answers.unity3d.com/questions/1170350/editorscript-generate-enum-from-string.html
					StringBuilder code = new StringBuilder();
					code.Append("using UnityEngine;\n\nnamespace ToryFramework.Scene\n{\n\tpublic abstract partial class TorySceneState : IToryScene\n\t{\n\t\tprotected void SetNext()\n\t\t{\n\t\t\tswitch (smb.Next)\n\t\t\t{\n\t\t\t\t");
					for (int i = 0; i < behaviour.States.Length; i++)
					{
						code.AppendFormat("case State.{0}:\n\t\t\t\t\tNext = Tory{1}Scene.Instance;\n\t\t\t\t\tbreak;\n\n\t\t\t\t", behaviour.States[i].ToUpper(), behaviour.States[i]);
					}
					code.Append("case State.EXIT:\n\t\t\t\tcase State.NULL:\n\t\t\t\t\tbreak;\n\n\t\t\t\tdefault:\n\t\t\t\t\tNext = null;\n\t\t\t\t\tDebug.LogError(\"[ToryScene] Please confirm the connection of ToryScene in the Animator. \" +\n\t\t\t\t\t               \"There is no valid next state in the \" + smb.Name + \" state.\");\n\t\t\t\t\tbreak;\n\t\t\t}\n\t\t}\n\t}\n}");

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
					path += "(Do Not Update This Folder)/Scripts/ToryScene/Core/TorySceneStatePartialNext.cs";

					// Write the script to the State.cs
					if (File.Exists(path))
					{
						if (!string.Equals(code.ToString(), File.ReadAllText(path)))
						{
							File.WriteAllText(path, code.ToString());
							AssetDatabase.ImportAsset(path);
							Debug.Log("[ToryScene] The \"TorySceneStatePartialNext.cs\" modified and loaded.");
						}
					}
					else
					{
						File.WriteAllText(path, code.ToString());
						AssetDatabase.ImportAsset(path);
						Debug.Log("[ToryScene] A new \"TorySceneStatePartialNext.cs\" created and loaded.");
					}
				}

				// 3. TorySceneBehaviourPartialStates.cs
				{
					// Modify the TorySceneBehaviourPartialStates.cs
					// Generating file reference: http://answers.unity3d.com/questions/1170350/editorscript-generate-enum-from-string.html
					StringBuilder code = new StringBuilder();
					code.Append("using System.Reflection;\nusing UnityEngine;\nusing ToryFramework.SMB;\n\nnamespace ToryFramework.Behaviour\n{\n\tpublic partial class TorySceneBehaviour : MonoBehaviour\n\t{\n\t\tvoid InitStates()\n\t\t{\n\t\t\t// Scene\n\t\t\tSystem.Type type = ToryScene.Instance.GetType();\n\t\t\tMethodInfo method = type.GetMethod(\"Init\", (BindingFlags.NonPublic | BindingFlags.Instance));\n\t\t\tif (method != null)\n\t\t\t{\n\t\t\t\tobject[] parameters = { animator.GetBehaviour<TorySceneSMB>() };\n\t\t\t\tmethod.Invoke(ToryScene.Instance, parameters);\n\t\t\t}");
					for (int i = 0; i < behaviour.States.Length; i++)
					{
						code.Append("\n\n\t\t\t// " + 
						            behaviour.States[i] + 
						            "\n\t\t\ttype = Tory" + 
						            behaviour.States[i] + 
						            "Scene.Instance.GetType();\n\t\t\tmethod = type.GetMethod(\"Init\", (BindingFlags.NonPublic | BindingFlags.Instance));\n\t\t\tif (method != null)\n\t\t\t{\n\t\t\t\tobject[] parameters = { animator.GetBehaviour<Tory" + 
						            behaviour.States[i] + 
						            "SceneSMB>() };\n\t\t\t\tmethod.Invoke(Tory" + 
						            behaviour.States[i] + 
						            "Scene.Instance, parameters);\n\t\t\t}");
					}
					code.Append("\n\t\t}\n\n\t\tvoid ResetEvents()\n\t\t{\n\t\t\t// Scene\n\t\t\tSystem.Type type = ToryScene.Instance.GetType();\n\t\t\tMethodInfo method = type.GetMethod(\"ResetEvents\", (BindingFlags.NonPublic | BindingFlags.Instance));\n\t\t\tif (method != null)\n\t\t\t{\n\t\t\t\tmethod.Invoke(ToryScene.Instance, null);\n\t\t\t}");
					for (int i = 0; i < behaviour.States.Length; i++)
					{
						code.Append("\n\n\t\t\t// " + 
						            behaviour.States[i] + 
						            "\n\t\t\ttype = Tory" + 
						            behaviour.States[i] + 
						            "Scene.Instance.GetType();\n\t\t\tmethod = type.GetMethod(\"ResetEvents\", (BindingFlags.NonPublic | BindingFlags.Instance));\n\t\t\tif (method != null)\n\t\t\t{\n\t\t\t\tmethod.Invoke(Tory" + 
						            behaviour.States[i] + 
						            "Scene.Instance, null);\n\t\t\t}");
					}
					code.Append("\n\t\t}\n\t}\n}");

					// Set the path of the TorySceneBehaviourPartialStates.cs
					MonoScript script = MonoScript.FromMonoBehaviour(behaviour);
					string path = AssetDatabase.GetAssetPath(script);
					path = path.Substring(0, path.Length - Path.GetFileName(path).Length - 1);
					string[] paths = path.Split('/');
					path = "";
					for (int i = 0; i < paths.Length - 3; i++)
					{
						path += paths[i] + "/";
					}
					path += "(Do Not Update This Folder)/Scripts/ToryScene/MonoBehaviours/TorySceneBehaviourPartialStates.cs";

					// Write the script to the State.cs
					if (File.Exists(path))
					{
						if (!string.Equals(code.ToString(), File.ReadAllText(path)))
						{
							File.WriteAllText(path, code.ToString());
							AssetDatabase.ImportAsset(path);
							Debug.Log("[ToryScene] The \"TorySceneBehaviourPartialStates.cs\" modified and loaded.");
						}
					}
					else
					{
						File.WriteAllText(path, code.ToString());
						AssetDatabase.ImportAsset(path);
						Debug.Log("[ToryScene] A new \"TorySceneBehaviourPartialStates.cs\" created and loaded.");
					}
				}

				// 4. ToryScenePartialState.cs
				{
					// Modify the ToryScenePartialState.cs
					// Generating file reference: http://answers.unity3d.com/questions/1170350/editorscript-generate-enum-from-string.html
					StringBuilder code = new StringBuilder();
					code.Append("using UnityEngine;\nusing ToryFramework.Scene;\n\nnamespace ToryFramework\n{\n\t/// <summary>\n\t/// Tory scene class that provides states and connections about the scene flow.\n\t/// You can manage a Unity scene by dividing it to multiple tory scenes.\n\t/// </summary>\n\tpublic partial class ToryScene : IToryScene\n\t{\n\t\tvoid SetFirst()\n\t\t{\n\t\t\tswitch (smb.First)\n\t\t\t{\n\t\t\t\t");
					for (int i = 0; i < behaviour.States.Length; i++)
					{
						code.AppendFormat("case State.{0}:\n\t\t\t\t\tFirst = Tory{1}Scene.Instance;\n\t\t\t\t\tbreak;\n\n\t\t\t\t", behaviour.States[i].ToUpper(), behaviour.States[i]);
					}
					code.Append("case State.NULL:\n\t\t\t\tcase State.EXIT:\n\t\t\t\tdefault:\n\t\t\t\t\tFirst = null;\n\t\t\t\t\tDebug.LogError(\"[ToryScene] Please confirm the connection of ToryScene in the Animator. \" +\n\t\t\t\t\t               \"There is the invalid first state in the ToryScene layer.\");\n\t\t\t\t\tbreak;\n\t\t\t}\n\t\t}\n");
					for (int i = 0; i < behaviour.States.Length; i++)
					{
						code.Append("\n\t\tpublic Tory" + 
						            behaviour.States[i] + 
						            "Scene " + 
						            behaviour.States[i] + 
						            " { get { return Tory" + 
						            behaviour.States[i] + 
						            "Scene.Instance; }}\n");
					}
					code.Append("\t}\n}");

					// Set the path of the TorySceneBehaviourPartialStates.cs
					MonoScript script = MonoScript.FromMonoBehaviour(behaviour);
					string path = AssetDatabase.GetAssetPath(script);
					path = path.Substring(0, path.Length - Path.GetFileName(path).Length - 1);
					string[] paths = path.Split('/');
					path = "";
					for (int i = 0; i < paths.Length - 3; i++)
					{
						path += paths[i] + "/";
					}
					path += "(Do Not Update This Folder)/Scripts/ToryScene/Singletons/ToryScenePartialState.cs";

					// Write the script to the State.cs
					if (File.Exists(path))
					{
						if (!string.Equals(code.ToString(), File.ReadAllText(path)))
						{
							File.WriteAllText(path, code.ToString());
							AssetDatabase.ImportAsset(path);
							Debug.Log("[ToryScene] The \"ToryScenePartialState.cs\" modified and loaded.");
						}
					}
					else
					{
						File.WriteAllText(path, code.ToString());
						AssetDatabase.ImportAsset(path);
						Debug.Log("[ToryScene] A new \"ToryScenePartialState.cs\" created and loaded.");
					}
				}

				// 5. ToryXXXScene.cs
				{
					// Set the path of the folder.
					MonoScript script = MonoScript.FromMonoBehaviour(behaviour);
					string path = AssetDatabase.GetAssetPath(script);
					path = path.Substring(0, path.Length - Path.GetFileName(path).Length - 1);
					string[] paths = path.Split('/');
					path = "";
					for (int j = 0; j < paths.Length - 3; j++)
					{
						path += paths[j] + "/";
					}
					path += "(Do Not Update This Folder)/Scripts/ToryScene/Singletons/Custom Scenes";

					// Delete files not existing in the path anymore.
					// Delete all files in the path: https://stackoverflow.com/questions/1288718/how-to-delete-all-files-and-folders-in-a-directory
					DirectoryInfo di = new DirectoryInfo(path);
					string targetFileName = "ToryXXXScene.cs";
					foreach (FileInfo file in di.GetFiles())
					{
						bool fileExist = false;
						for (int i = 0; i < behaviour.States.Length; i++)
						{
							targetFileName = "Tory" + behaviour.States[i] + "Scene.cs";
							if (string.Equals(targetFileName, file.Name))
							{
								fileExist = true;
								break;
							}
						}
						if (!fileExist)
						{
							file.Delete();
						}
					}
					path += "/ToryXXXScene.cs";

					StringBuilder code = new StringBuilder();
					for (int i = 0; i < behaviour.States.Length; i++)
					{
						// Modify the ToryXXXScene.cs
						// Generating file reference: http://answers.unity3d.com/questions/1170350/editorscript-generate-enum-from-string.html
						code.Remove(0, code.Length);
						code.Append("using ToryFramework.Scene;\n\nnamespace ToryFramework\n{\n\tpublic class Tory" + 
						            behaviour.States[i] + 
						            "Scene : TorySceneState\n\t{\n\t\t#region SINGLETON\n\n\t\tstatic volatile Tory" + 
						            behaviour.States[i] + 
						            "Scene instance;\n\t\tstatic readonly object syncRoot = new object();\n\n\t\tTory" + 
						            behaviour.States[i] + 
						            "Scene() { }\n\n\t\tpublic static Tory" + 
						            behaviour.States[i] + 
						            "Scene Instance\n\t\t{\n\t\t\tget\n\t\t\t{\n\t\t\t\tif (instance == null)\n\t\t\t\t{\n\t\t\t\t\tlock (syncRoot)\n\t\t\t\t\t{\n\t\t\t\t\t\tif (instance == null)\n\t\t\t\t\t\t{\n\t\t\t\t\t\t\tinstance = new Tory" + 
						            behaviour.States[i] + 
						            "Scene();\n\t\t\t\t\t\t}\n\t\t\t\t\t}\n\t\t\t\t}\n\t\t\t\treturn instance;\n\t\t\t}\n\t\t}\n\n\t\t#endregion\n\t}\n}");

						// Set the path of the ToryXXXScene.cs
						path = path.Substring(0, path.Length - Path.GetFileName(path).Length);
						path += "Tory" + behaviour.States[i] + "Scene.cs";

						// Write the script to the ToryXXXScene.cs
						if (File.Exists(path))
						{
							if (!string.Equals(code.ToString(), File.ReadAllText(path)))
							{
								File.WriteAllText(path, code.ToString());
								AssetDatabase.ImportAsset(path);
								Debug.Log("[ToryScene] A new \"" + Path.GetFileName(path) + "\" modified and loaded.");
							}
						}
						else
						{
							File.WriteAllText(path, code.ToString());
							AssetDatabase.ImportAsset(path);
							Debug.Log("[ToryScene] A new \"" + Path.GetFileName(path) + "\" created and loaded.");
						}
					}
				}

				// 6. ToryXXXSceneSMB.cs
				{
					// Set the path of the folder.
					MonoScript script = MonoScript.FromMonoBehaviour(behaviour);
					string path = AssetDatabase.GetAssetPath(script);
					path = path.Substring(0, path.Length - Path.GetFileName(path).Length - 1);
					string[] paths = path.Split('/');
					path = "";
					for (int j = 0; j < paths.Length - 3; j++)
					{
						path += paths[j] + "/";
					}
					path += "(Do Not Update This Folder)/Scripts/ToryScene/StateMachineBehaviours/Custom SMBs";

					// Delete files not existing in the path anymore.
					// Delete all files in the path: https://stackoverflow.com/questions/1288718/how-to-delete-all-files-and-folders-in-a-directory
					DirectoryInfo di = new DirectoryInfo(path);
					string targetFileName = "ToryXXXSceneSMB.cs";
					foreach (FileInfo file in di.GetFiles())
					{
						bool fileExist = false;
						for (int i = 0; i < behaviour.States.Length; i++)
						{
							targetFileName = "Tory" + behaviour.States[i] + "SceneSMB.cs";
							if (string.Equals(targetFileName, file.Name))
							{
								fileExist = true;
								break;
							}
						}
						if (!fileExist)
						{
							file.Delete();
						}
					}
					path += "/ToryXXXSceneSMB.cs";

					StringBuilder code = new StringBuilder();
					for (int i = 0; i < behaviour.States.Length; i++)
					{
						// Modify the ToryXXXScene.cs
						// Generating file reference: http://answers.unity3d.com/questions/1170350/editorscript-generate-enum-from-string.html
						code.Remove(0, code.Length);
						code.Append("namespace ToryFramework.SMB\n{\n\tpublic class Tory" + 
						            behaviour.States[i] + 
						            "SceneSMB : ToryCustomSceneSMB\n\t{\n\t\tpublic override string Name \t\t\t{ get { return \"" + 
						            behaviour.States[i] + 
						            "\"; }}\n\t}\n}");

						// Set the path of the ToryXXXScene.cs
						path = path.Substring(0, path.Length - Path.GetFileName(path).Length);
						path += "Tory" + behaviour.States[i] + "SceneSMB.cs";

						// Write the script to the ToryXXXScene.cs
						if (File.Exists(path))
						{
							if (!string.Equals(code.ToString(), File.ReadAllText(path)))
							{
								File.WriteAllText(path, code.ToString());
								AssetDatabase.ImportAsset(path);
								Debug.Log("[ToryScene] A new \"" + Path.GetFileName(path) + "\" modified and loaded.");
							}
						}
						else
						{
							File.WriteAllText(path, code.ToString());
							AssetDatabase.ImportAsset(path);
							Debug.Log("[ToryScene] A new \"" + Path.GetFileName(path) + "\" created and loaded.");
						}
					}
				}

				// 7. ToryXXXSceneSMBEditor.cs
				{
					// Set the path of the folder.
					MonoScript script = MonoScript.FromMonoBehaviour(behaviour);
					string path = AssetDatabase.GetAssetPath(script);
					path = path.Substring(0, path.Length - Path.GetFileName(path).Length - 1);
					string[] paths = path.Split('/');
					path = "";
					for (int j = 0; j < paths.Length - 3; j++)
					{
						path += paths[j] + "/";
					}
					path += "(Do Not Update This Folder)/Scripts/ToryScene/StateMachineBehaviours/Custom SMBs/Editor";

					// Delete files not existing in the path anymore.
					// Delete all files in the path: https://stackoverflow.com/questions/1288718/how-to-delete-all-files-and-folders-in-a-directory
					DirectoryInfo di = new DirectoryInfo(path);
					string targetFileName = "ToryXXXSceneSMBEditor.cs";
					foreach (FileInfo file in di.GetFiles())
					{
						bool fileExist = false;
						for (int i = 0; i < behaviour.States.Length; i++)
						{
							targetFileName = "Tory" + behaviour.States[i] + "SceneSMBEditor.cs";
							if (string.Equals(targetFileName, file.Name))
							{
								fileExist = true;
								break;
							}
						}
						if (!fileExist)
						{
							file.Delete();
						}
					}
					path += "/ToryXXXSceneSMBEditor.cs";

					StringBuilder code = new StringBuilder();
					for (int i = 0; i < behaviour.States.Length; i++)
					{
						// Modify the ToryXXXScene.cs
						// Generating file reference: http://answers.unity3d.com/questions/1170350/editorscript-generate-enum-from-string.html
						code.Remove(0, code.Length);
						code.Append("using UnityEditor;\nusing ToryFramework.SMB;\n\nnamespace ToryFramework.Editor\n{\n\t[CustomEditor(typeof(Tory" + 
						            behaviour.States[i] + 
						            "SceneSMB))]\n\t[CanEditMultipleObjects]\n\tpublic class Tory" + 
						            behaviour.States[i] + 
						            "SceneSMBEditor : ToryCustomSceneSMBEditor\n\t{\n\t\t#region PROPERTIES\n\n\t\tprotected override string Name \t\t\t{ get { return \"" + 
						            behaviour.States[i].ToUpper() + 
						            "\"; }}\n\n\t\t#endregion\n\t}\n}");

						// Set the path of the ToryXXXScene.cs
						path = path.Substring(0, path.Length - Path.GetFileName(path).Length);
						path += "Tory" + behaviour.States[i] + "SceneSMBEditor.cs";

						// Write the script to the ToryXXXScene.cs
						if (File.Exists(path))
						{
							if (!string.Equals(code.ToString(), File.ReadAllText(path)))
							{
								File.WriteAllText(path, code.ToString());
								AssetDatabase.ImportAsset(path);
								Debug.Log("[ToryScene] A new \"" + Path.GetFileName(path) + "\" modified and loaded.");
							}
						}
						else
						{
							File.WriteAllText(path, code.ToString());
							AssetDatabase.ImportAsset(path);
							Debug.Log("[ToryScene] A new \"" + Path.GetFileName(path) + "\" created and loaded.");
						}
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
		public string CheckClassName(string str)
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

		#endregion
	}	
}
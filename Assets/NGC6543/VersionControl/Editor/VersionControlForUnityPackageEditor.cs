using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace NGC6543
{
	[CustomEditor(typeof(VersionControlForUnityPackage))]
	public class VersionControlForUnityPackageEditor : VersionControlBaseEditor 
	{
		SerializedProperty objectsToBeExported;
		SerializedProperty packageName;
		SerializedProperty exportNumber;
		SerializedProperty exportedPackagePath;
		SerializedProperty additionalTag;
		SerializedProperty additionalTagPosition;
		SerializedProperty releaseNote;
		
		
		VersionControlForUnityPackage _component1;
		
		bool setExportNumberManually;
		
		new void OnEnable()
		{
			base.OnEnable();
			
			_component1 = target as VersionControlForUnityPackage;
			
			objectsToBeExported = serializedObject.FindProperty("_objectsToBeExported");
			packageName = serializedObject.FindProperty("_packageName");
			exportNumber = serializedObject.FindProperty("_exportNumber");
			exportedPackagePath = serializedObject.FindProperty("_exportedPackagePath");
			additionalTag = serializedObject.FindProperty("_additionalTag");
			additionalTagPosition = serializedObject.FindProperty("_additionalTagPosition");
			releaseNote = serializedObject.FindProperty("_releaseNote");
			
		}
		
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            //=== Begins VersionControlForUnityPackageEditor
            EditorGUILayout.LabelField("Export Settings");

            EditorGUILayout.Space();
            EditorGUILayout.Space();
			
			EditorGUILayout.PropertyField(objectsToBeExported, new GUIContent("Objects to be exported"), true);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
			
			EditorGUILayout.LabelField("Export Number", exportNumber.intValue.ToString());
			setExportNumberManually = EditorGUILayout.ToggleLeft("Set Export Number Manually", setExportNumberManually);
			if (setExportNumberManually)
			{
				EditorGUILayout.PropertyField(exportNumber, new GUIContent("Export Number"));
			}
			
			EditorGUILayout.Space();
			
			EditorGUILayout.PropertyField(exportedPackagePath, new GUIContent("Exported Package Path"));
			
			// EditorGUILayout.LabelField("Export Path", exportedPackagePath.stringValue);
			// if (GUILayout.Button("Change Folder"))
			// {
			// 	_component1.ChangeExportFolderPath();
			// }
			
			EditorGUILayout.Space();
			
			EditorGUILayout.PropertyField(packageName, new GUIContent("Package Name"));
			
			EditorGUILayout.PropertyField(additionalTag, new GUIContent("Additional Tag"));
			
			EditorGUILayout.PropertyField(additionalTagPosition, new GUIContent("Additional Tag Position"));

			EditorGUILayout.LabelField("The file name will be : ", _component1.GetExportedPackageName());
			
            EditorGUILayout.Space();
			
			EditorGUILayout.PropertyField(releaseNote, new GUIContent("Release Note"));

            EditorGUILayout.Space();
			

            EditorGUILayout.Space();
			
			if (GUILayout.Button("Export to UnityPackage"))
			{
				_component1.ExportUnityPackage();
				
			}
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}

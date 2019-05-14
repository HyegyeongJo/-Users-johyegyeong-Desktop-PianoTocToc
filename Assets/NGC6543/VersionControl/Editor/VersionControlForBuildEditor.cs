using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NGC6543
{
	[CustomEditor(typeof(VersionControlForBuild))]
	public class VersionControlForBuildEditor : VersionControlBaseEditor
	{
		SerializedProperty buildNumber;
		
		SerializedProperty overrideActiveBuildTarget;
		SerializedProperty buildTarget;
		
		SerializedProperty builtProjectPath;
		SerializedProperty additionalTag;
		SerializedProperty additionalTagPosition;
		SerializedProperty isDevBuild;
		SerializedProperty doNotIncreaseBuildNumber;
		SerializedProperty releaseNote;
		SerializedProperty addiObjs;
		
		SerializedProperty exportToZip;
		
		//=== Member variables
		VersionControlForBuild _component1;
		int _major, _minor, _patch;
		
		//=== Flags
		bool toggleSetBuildNumberManually;
				
		new void OnEnable()
		{
			base.OnEnable();
			_component1 = target as VersionControlForBuild;

            buildNumber = serializedObject.FindProperty("_buildNumber");
			
			overrideActiveBuildTarget = serializedObject.FindProperty("_overrideActiveBuildTarget");
			buildTarget = serializedObject.FindProperty("_buildTarget");
			
            builtProjectPath = serializedObject.FindProperty("_builtProjectPath");
            additionalTag = serializedObject.FindProperty("_additionalTag");
			additionalTagPosition = serializedObject.FindProperty("_additionalTagPosition");
            isDevBuild = serializedObject.FindProperty("_isDevBuild");
            doNotIncreaseBuildNumber = serializedObject.FindProperty("_doNotIncreaseBuildNumber");
            releaseNote = serializedObject.FindProperty("_releaseNote");
			addiObjs = serializedObject.FindProperty("_additionalObjectsToBeCopied");
			
            exportToZip = serializedObject.FindProperty("_exportToZip");
			
			_major = major.intValue;
			_minor = minor.intValue;
			_patch = patch.intValue;
		
		}
		
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (_major != major.intValue || _minor != minor.intValue || _patch != patch.intValue)
			{
                _major = major.intValue;
                _minor = minor.intValue;
                _patch = patch.intValue;
				_component1.UpdateVersion();
			}
			
			serializedObject.Update();
			
			EditorGUILayout.Space();
            EditorGUILayout.Space();
			
			/*************************************************************
			//						BUILD SETTINGS
			*************************************************************/
			EditorGUILayout.LabelField("Build Settings");
			
			EditorGUILayout.HelpBox("This asset may not work properly if its name had been changed, or its location had been moved!\n Please keep it as it was created! (Assets/_VersionControlForBuild)", MessageType.Warning);
			
			// BUILD NUMBER
			
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Build Number", _component1.BuildNumber.ToString());
			toggleSetBuildNumberManually = EditorGUILayout.ToggleLeft("Set Build Number Manually", toggleSetBuildNumberManually);
			if (toggleSetBuildNumberManually)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(buildNumber);
				if (EditorGUI.EndChangeCheck())
				{
					_component1.SetBuildNumber(buildNumber.intValue);
				}
			}
			
			// BUILD TARGET
			
			EditorGUILayout.Space();
			EditorGUILayout.PrefixLabel("Build Target");
			EditorGUILayout.HelpBox("You can change the Build Target here. If set, the active target platform will be switched to the selected one. The build time may take longer due to asset re-import.", MessageType.Info);
			
			overrideActiveBuildTarget.boolValue = EditorGUILayout.ToggleLeft("Override Build Target", overrideActiveBuildTarget.boolValue);
			if (overrideActiveBuildTarget.boolValue)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(buildTarget, new GUIContent("Build Target"));
				EditorGUI.indentLevel--;
			}
			
			// BUILT FILE PATH AND RELEASE NOTE
			
			EditorGUILayout.Space();
			
            EditorGUILayout.PropertyField(builtProjectPath, new GUIContent("Built Project Path"));
			
			EditorGUILayout.PropertyField(additionalTag, new GUIContent("Additional Tag"));
			
			EditorGUILayout.PropertyField(additionalTagPosition, new GUIContent("Additional Tag Position"));
			
			EditorGUILayout.LabelField("The file name will be : ", _component1.GetBuiltProjectName(isDevBuild.boolValue));
			
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			
			
			// Additional files to be copied
			EditorGUILayout.PrefixLabel("Additional files to be copied");
			EditorGUI.indentLevel++;
			
			// Release Note
			EditorGUILayout.ObjectField(releaseNote, new GUIContent("Release Note"));
			if (releaseNote.objectReferenceValue != null)
			{
				EditorGUILayout.HelpBox("The release note will be copied(or overwritten) to the built project folder with a version and date(YYMMDD) tag after its original name. Both Files and Folders are accepted.\n"
				+ "File name will be : FileName_v[CurrentVersion]_YYMMDD.Extension", MessageType.Info);
			}
			
			// Additional Objects to be copied
			EditorGUILayout.HelpBox("Additional Objects will be copied to the output path after a successful build. Both Files and Folders are accepted.", MessageType.Info);
			addiObjs.isExpanded = EditorGUILayout.Foldout(addiObjs.isExpanded, "Additional objects to be copied");
			if (addiObjs.isExpanded)
			{
				EditorGUI.indentLevel++;
				addiObjs.arraySize = EditorGUILayout.DelayedIntField(new GUIContent("Count"),addiObjs.arraySize);
				
				for (int i = 0; i < addiObjs.arraySize; i++)
				{
					EditorGUILayout.ObjectField(addiObjs.GetArrayElementAtIndex(i), new GUIContent("Object " + i));
				}
				EditorGUI.indentLevel--;
			}
			EditorGUI.indentLevel--;
			
			
			EditorGUILayout.Space();
			
			EditorGUILayout.PropertyField(isDevBuild, new GUIContent("Is Development Build"));
			if (isDevBuild.boolValue)
			{
				EditorGUILayout.HelpBox("Development build : Auto connect profiler is enabled.", MessageType.Info);
			}
			
			EditorGUILayout.PropertyField(doNotIncreaseBuildNumber, new GUIContent("Do not increase Build Number"));
			if (doNotIncreaseBuildNumber.boolValue)
			{
				EditorGUILayout.HelpBox("Build Number won't automatically be increased after a successful build!", MessageType.Warning);
			}
			
			//UNDONE NOT IMPLEMENTED YET
			// EditorGUILayout.PropertyField(exportToZip, new GUIContent("Export to *.zip"));
			// if (exportToZip.boolValue)
			// {
            //     EditorGUILayout.HelpBox("NOT IMPLEMENTED YET", MessageType.Error);
			// 	// EditorGUILayout.HelpBox("The built project and the release note will be compressed into a single *.zip file.", MessageType.Info);
			// }
			
			
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			
			if (overrideActiveBuildTarget.boolValue)
			{
				EditorGUILayout.HelpBox("The build target has been overriden! Check before build!", MessageType.Warning);
			}
			if (GUILayout.Button("BUILD PROJECT"))
			{
				_component1.Build();
			}
			
			serializedObject.ApplyModifiedProperties();
			
		}
	}
	
}

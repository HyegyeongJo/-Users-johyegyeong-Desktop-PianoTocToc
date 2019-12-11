using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
#endif

#if UNITY_EDITOR
namespace NGC6543
{
	public struct BuildPlayerOptionsPreset
	{
		public BuildTarget targetPlatform;
		
		public SceneAsset[] scenesToBuild;
		
		public string builtProjectPath;
		
		public string additionalTag;
		
		public VersionControlForBuild.AdditionalTagPosition additionalTagPosition;
		
		public bool isDevBuild;
		
		public bool doNotIncreaseBuildNumber;
		
		public Object releaseNote;
	}
	
	
	public class BuildPlayerOptionsPresetPropertyDrawer : PropertyDrawer
	{
		SerializedProperty targetPlatform;
		SerializedProperty scenesToBuild;
		SerializedProperty builtProjectPath;
		SerializedProperty additionalTag;
		SerializedProperty additionalTagPosition;
		SerializedProperty isDevBuild;
		SerializedProperty doNotIncreaseBuildNumber;
		SerializedProperty releaseNote;
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			targetPlatform = property.FindPropertyRelative("targetPlatform");
			scenesToBuild = property.FindPropertyRelative("scenesToBuild");
			builtProjectPath = property.FindPropertyRelative("builtProjectPath");
			additionalTag = property.FindPropertyRelative("additionalTag");
			additionalTagPosition = property.FindPropertyRelative("additionalTagPosition");
			isDevBuild = property.FindPropertyRelative("isDevBuild");
			doNotIncreaseBuildNumber = property.FindPropertyRelative("doNotIncreaseBuildNumber");
			releaseNote = property.FindPropertyRelative("releaseNote");
			
			
		}
	}
}
#endif

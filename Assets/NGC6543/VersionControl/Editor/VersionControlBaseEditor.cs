using UnityEngine;
using UnityEditor;

namespace NGC6543
{
    [CustomEditor(typeof(VersionControlBase))]
    public class VersionControlBaseEditor : Editor
    {   
        protected SerializedProperty major, minor, patch;
        
        VersionControlBase myScript0;
        
        //=== Flags
        bool toggleSetVersionManually;
        
        protected void OnEnable()
        {
            myScript0 = target as VersionControlBase;
            
            major = serializedObject.FindProperty("_major");
            minor = serializedObject.FindProperty("_minor");
            patch = serializedObject.FindProperty("_patch");
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("SEMANTIC VERSION CONTROL HELPER v" + VersionControlBase.API_VERSION 
                + "\nby NGC6543\n\nSemantic Versioning 2.0.0-k0 (https://semver.org)", MessageType.Info);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Current Version", myScript0.CurrentVersion);
            
            serializedObject.Update();
            
            //=== Manually overrides version
            toggleSetVersionManually = EditorGUILayout.ToggleLeft("Set Version Manually", toggleSetVersionManually);
            if (toggleSetVersionManually)
            {
                EditorGUILayout.PropertyField(major, new GUIContent("Major"));
                EditorGUILayout.PropertyField(minor, new GUIContent("Minor"));
                EditorGUILayout.PropertyField(patch, new GUIContent("Patch"));
            }
            
            //=== Version Increment
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Major"))
            {
                myScript0.IncrementMajor();
            }
            if (GUILayout.Button("Minor"))
            {
                myScript0.IncrementMinor();
            }
            if (GUILayout.Button("Patch"))
            {
                myScript0.IncrementPatch();
            }
            EditorGUILayout.EndHorizontal();
            
            serializedObject.ApplyModifiedProperties();
        }
        
    }
}
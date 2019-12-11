using UnityEditor;
using UnityEditor.UI;

namespace LeTai.Asset.TranslucentImage.Editor
{
    [CustomEditor(typeof(TranslucentImage))]
    [CanEditMultipleObjects]
    public class TranslucentImageEditor : ImageEditor
    {
        SerializedProperty source;
        SerializedProperty vibrancy;
        SerializedProperty brightness;
        SerializedProperty flatten;

        protected override void OnEnable()
        {
            base.OnEnable();

            source = serializedObject.FindProperty("source");
            vibrancy = serializedObject.FindProperty("vibrancy");
            brightness = serializedObject.FindProperty("brightness");
            flatten = serializedObject.FindProperty("flatten");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        
            EditorGUILayout.Space();
        
            serializedObject.Update();
            EditorGUILayout.PropertyField(source);
            EditorGUILayout.PropertyField(vibrancy);
            EditorGUILayout.PropertyField(brightness);
            EditorGUILayout.PropertyField(flatten);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
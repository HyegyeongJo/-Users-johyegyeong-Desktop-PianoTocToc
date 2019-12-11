using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace ToryUX
{
    [CustomEditor(typeof(ToryTextBox))]
    public class ToryTextBoxEditor : Editor
    {
        ToryTextBox component;

        void OnEnable()
        {
            component = (ToryTextBox)target;
        }

        public override void OnInspectorGUI()
        {
            if (component.uiText != null)
            {
                EditorGUILayout.Space();
                EditorGUI.BeginChangeCheck();
                Undo.RecordObject(component.uiText, "Text change");
                component.uiText.text = EditorGUILayout.TextArea(component.uiText.text, GUILayout.ExpandHeight(true));
                if (EditorGUI.EndChangeCheck())
                {
                    EditorApplication.delayCall += component.ResizeToFit;
                }
            }
        }
    }
}
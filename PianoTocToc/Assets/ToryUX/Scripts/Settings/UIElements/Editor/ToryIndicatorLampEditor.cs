using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace ToryUX
{
    [CustomEditor(typeof(ToryIndicatorLamp))]
    public class ToryIndicatorLampEditor : Editor
    {
        SerializedProperty currentIndicatorLampState;
        SerializedProperty showDifferentMessageForDifferentState;
        SerializedProperty label;
        SerializedProperty offMessage;
        SerializedProperty greenMessage;
        SerializedProperty yellowMessage;
        SerializedProperty redMessage;

        void OnEnable()
        {
            currentIndicatorLampState = serializedObject.FindProperty("currentIndicatorLampState");
            showDifferentMessageForDifferentState = serializedObject.FindProperty("showDifferentMessageForDifferentState");
            label = serializedObject.FindProperty("label");
            offMessage = serializedObject.FindProperty("offMessage");
            greenMessage = serializedObject.FindProperty("greenMessage");
            yellowMessage = serializedObject.FindProperty("yellowMessage");
            redMessage = serializedObject.FindProperty("redMessage");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            try
            {
                EditorGUILayout.PropertyField(currentIndicatorLampState);
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(showDifferentMessageForDifferentState);
                if (showDifferentMessageForDifferentState.boolValue)
                {
                    EditorGUILayout.PropertyField(offMessage);
                    EditorGUILayout.PropertyField(greenMessage);
                    EditorGUILayout.PropertyField(yellowMessage);
                    EditorGUILayout.PropertyField(redMessage);
                }
                else
                {
                    EditorGUILayout.PropertyField(label);
                }
            }
            catch (System.NullReferenceException)
            {
                EditorGUILayout.HelpBox("ToryIndicatorLamp does not have properly named child objects.", MessageType.Error);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
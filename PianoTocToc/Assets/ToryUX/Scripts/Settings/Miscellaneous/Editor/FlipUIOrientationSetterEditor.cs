using UnityEditor;
using UnityEngine;

namespace ToryUX
{
    [CustomEditor(typeof(FlipUIOrientationSetter))]
    public class FlipUIOrientationSetterEditor : Editor
    {
        FlipUIOrientationSetter component;

        SerializedProperty defaultValue;

        bool prevValue;

        void OnEnable()
        {
            component = (FlipUIOrientationSetter) target;
            component.OnEnable();

            defaultValue = serializedObject.FindProperty("defaultValue");

            prevValue = component.ToggleComponent.isOn;

            ToryValue.SecureKeysChecker.CheckSecureKeys();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();

            if (prevValue != component.ToggleComponent.isOn)
            {
                component.FlipUIOrientation(component.ToggleComponent.isOn);
                prevValue = component.ToggleComponent.isOn;
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(defaultValue);
            if (EditorGUI.EndChangeCheck())
            {
                UIOrientationSetter.IsFlipped.DefaultValue = defaultValue.boolValue;
                component.FlipUIOrientation(defaultValue.boolValue);
            }

            if (GUILayout.Button("Revert to Default Settings"))
            {
                ((IDefaultValueSetter) target).RevertToDefault();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}